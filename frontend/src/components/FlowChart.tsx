import React, { useMemo } from 'react';
import ReactFlow, { 
  Background, 
  Controls, 
  type Edge, 
  type Node, 
  MarkerType 
} from 'reactflow';
import 'reactflow/dist/style.css';
import type { Variable, BalanceResponse } from '../types';

interface Props {
  variables: Variable[];
  results: BalanceResponse | null;
}

const nodeStyle: React.CSSProperties = {
  width: 60,
  height: 60,
  borderRadius: '50%',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  border: '2px solid #333',
  fontWeight: 'bold',
  backgroundColor: '#fff',
};

export const FlowChart: React.FC<Props> = ({ variables, results }) => {
  const { nodes, edges } = useMemo(() => {
    const nodes: Node[] = [];
    const edges: Edge[] = [];
    const nodeSet = new Set<string>();

    // 1. Собираем уникальные ID узлов
    variables.forEach(v => {
      if (v.sourceId) nodeSet.add(v.sourceId);
      if (v.destinationId) nodeSet.add(v.destinationId);
    });

    const sortedNodeIds = Array.from(nodeSet).sort();
    
    // Счетчик выходов для каждого узла (чтобы X2, X4, X6 не слипались)
    const nodeOutputCounters: Record<string, number> = {};

    // 2. Создаем основные узлы
    sortedNodeIds.forEach((id, index) => {
      nodes.push({
        id,
        data: { label: id.length > 5 ? `${index + 1}` : id }, 
        position: { x: index * 350 + 200, y: 200 },
        style: nodeStyle,
      });
      nodeOutputCounters[id] = 0;
    });

    // 3. Создаем связи
    variables.forEach((v) => {
      const reconciled = results?.reconciledVariables.find(r => r.id === v.id);
      const valText = reconciled 
        ? reconciled.reconciledValue.toFixed(2) 
        : v.measured.toFixed(2);

      const label = `${v.name}: ${valText}`;

      // СЛУЧАЙ А: Поток между узлами (X3, X5)
      if (v.sourceId && v.destinationId) {
        edges.push({
          id: v.id,
          source: v.sourceId,
          target: v.destinationId,
          label,
          markerEnd: { type: MarkerType.ArrowClosed, color: '#333' },
          style: { stroke: '#333', strokeWidth: 2 },
          animated: !!results,
        });
      } 
      // СЛУЧАЙ Б: Входящий поток (X1)
      else if (!v.sourceId && v.destinationId) {
        const targetIdx = sortedNodeIds.indexOf(v.destinationId);
        const inputId = `in-${v.id}`;
        nodes.push({
          id: inputId,
          data: { label: '' },
          // Смещаем точку входа левее и на уровень центра узла
          position: { x: targetIdx * 350 + 50, y: 230 }, 
          style: { width: 0, height: 0, opacity: 0 },
        });
        edges.push({
          id: v.id,
          source: inputId,
          target: v.destinationId,
          label,
          type: 'straight', // Делаем линию прямой
          markerEnd: { type: MarkerType.ArrowClosed, color: '#333' },
          style: { stroke: '#333', strokeWidth: 2 },
        });
      } 
      // СЛУЧАЙ В: Исходящий поток (X2, X4, X6, X7...)
      else if (v.sourceId && !v.destinationId) {
        const sourceIdx = sortedNodeIds.indexOf(v.sourceId);
        const outputId = `out-${v.id}`;
        
        const count = nodeOutputCounters[v.sourceId]++;
        const direction = count % 2 === 0 ? 1 : -1;
        const offset = 150 * direction;

        nodes.push({
          id: outputId,
          data: { label: '' },
          // Смещаем точку выхода так, чтобы она была строго под/над узлом или чуть сбоку
          position: { x: sourceIdx * 350 + 230, y: 230 + offset },
          style: { width: 0, height: 0, opacity: 0 },
        });

        edges.push({
          id: v.id,
          source: v.sourceId,
          target: outputId,
          label,
          type: 'straight', // Делаем линию прямой
          markerEnd: { type: MarkerType.ArrowClosed, color: '#333' },
          style: { stroke: '#333', strokeWidth: 2 },
        });
      }
    });

    return { nodes, edges };
  }, [variables, results]);

  return (
    <div style={{ width: '100%', height: '500px', background: '#fff', border: '1px solid #ddd' }}>
      <ReactFlow nodes={nodes} edges={edges} fitView>
        <Background color="#ccc" gap={20} />
        <Controls />
      </ReactFlow>
    </div>
  );
};