import React from 'react';
import type { Variable } from '../types';

interface Props {
  variables: Variable[];
  onRemove: (id: string) => void;
}

export const VariablesTable: React.FC<Props> = ({ variables, onRemove }) => {
  if (variables.length === 0) return <p>Нет добавленных потоков.</p>;

  return (
    <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%', marginBottom: '20px' }}>
      <thead style={{ backgroundColor: '#f5f5f5' }}>
        <tr>
          <th>Имя</th><th>Источник</th><th>Приемник</th><th>Измерение</th><th>Погрешность</th><th>Действия</th>
        </tr>
      </thead>
      <tbody>
        {variables.map(v => (
          <tr key={v.id}>
            <td>{v.name}</td><td>{v.sourceId || '-'}</td><td>{v.destinationId || '-'}</td><td>{v.measured}</td><td>{v.tolerance}</td>
            <td><button onClick={() => onRemove(v.id)} style={{ color: 'red' }}>Удалить</button></td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};