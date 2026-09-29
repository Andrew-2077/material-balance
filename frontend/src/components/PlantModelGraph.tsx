import React, { useMemo } from "react";
import ReactFlow, {
    MiniMap,
    Background,
    Controls,
    type Edge,
    type Node,
    MarkerType
} from 'reactflow';
import 'reactflow/dist/style.css';
import type { Variable, BalanceResponse } from '../types';

interface Props {
    originalVariables: Variable[];
    results?: BalanceResponse | null;
}

export const PlantModelGraph: React.FC<Props> = ({ originalVariables, results }) => {
    const { nodes, edges } = useMemo(() => {
        const newNodes: Node[] = [];
        const newEdges: Edge[] = [];
        const uniqueNodeIds = new Set<string>();

        originalVariables.forEach((v) => {
            const source = v.sourceId && String(v.sourceId) !== 'null' ? String(v.sourceId) : 'EXTERNAL_IN';
            const target = v.destinationId && String(v.destinationId) !== 'null' ? String(v.destinationId) : 'EXTERNAL_OUT';

            uniqueNodeIds.add(source);
            uniqueNodeIds.add(target);

            const reconciledData = results?.reconciledVariables?.find(
                (r) => String(r.id) === String(v.id)
            );

            // Грубые ошибки
            const isGrossError = reconciledData?.isGrossError || results?.grossErrorStreams?.includes(v.id) || false;

            const flowValue = reconciledData
                ? reconciledData.reconciledValue?.toFixed(3)
                : v.measured?.toFixed(3) ?? '?';

            const label = isGrossError
                ? `⚠️ ${v.name}\n(${flowValue})`
                : `${v.name}\n(${flowValue})`;

            const strokeColor = isGrossError
                ? '#ef4444' // Красный (Gross Error)
                : (reconciledData ? '#10b981' : '#3b82f6'); // Зеленый или Синий

            newEdges.push({
                id: `e-${v.id}`,
                source: source,
                target: target,
                label: label,
                animated: true,
                labelStyle: {
                    fill: isGrossError ? '#ef4444' : '#333',
                    fontWeight: 700,
                    fontSize: '12px'
                },
                style: {
                    stroke: strokeColor,
                    strokeWidth: isGrossError ? 5 : 2,
                    strokeDasharray: isGrossError ? '5 5' : 'none'
                },
                markerEnd: {
                    type: MarkerType.ArrowClosed,
                    color: strokeColor
                },
            });
        });

        Array.from(uniqueNodeIds).forEach((id, index) => {
            if (id === 'null' || !id) return;

            let labels = `Узел ${id}`;
            let type = 'default';
            let bgColor = '#fff';

            if (id === 'EXTERNAL_IN') {
                labels = 'Вход системы';
                type = 'input',
                    bgColor = '#e0f2fe';
            } else if (id === 'EXTERNAL_OUT') {
                labels = 'Выход системы';
                type = 'output',
                    bgColor = '#fef2f2';
            }

            newNodes.push({
                id: id,
                position: {
                    x: (index % 4) * 200,
                    y: Math.floor(index / 4) * 150
                },
                data: { label: labels },
                type: type,
                style: {
                    background: bgColor,
                    border: '1px solid #333',
                    borderRadius: '5px',
                    fontWeight: 'bold',
                    fontSize: '12px',
                    width: 100
                },
            });
        });

        return { nodes: newNodes, edges: newEdges };

    }, [originalVariables, results]);

    if (originalVariables.length === 0) {
        return <div>Нет данных для отрисовки модели...</div>;
    }

    return (
        <div style={{ height: '500px', width: '100%', border: '1px solid #ccc', borderRadius: '8px', marginTop: '20px' }}>
            <ReactFlow
                nodes={nodes}
                edges={edges}
                fitView
            >
                <Controls />
                <MiniMap />
                <Background gap={16} size={1} />
            </ReactFlow>
        </div>
    );
};