import React from 'react';
import type { BalanceResponse } from '../types';

interface Props {
  data: BalanceResponse | null;
}

export const ResultsTable: React.FC<Props> = ({ data }) => {
  if (!data || !data.reconciledVariables) return null;

  const isSuccess = data.status === 'FOUND_IN_BOUNDS';

  // Стиль глобального теста
  const isGtPassed = data.isGlobalTestPassed;
  const gtColor = isGtPassed ? '#28a745' : '#dc3545';
  const gtBg = isGtPassed ? '#d4edda' : '#f8d7da';

  return (
    <div style={{ marginTop: '30px', padding: '20px', border: `2px solid ${isSuccess ? '#28a745' : '#dc3545'}`, borderRadius: '8px' }}>
      <h3>Результаты сведения (Статус: {data.status})</h3>
      <div style={{
        padding: '12px',
        backgroundColor: gtBg,
        color: gtColor,
        borderRadius: '6px',
        marginBottom: '20px',
        border: `1px solid ${gtColor}`
      }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span style={{ fontWeight: 'bold', fontSize: '1.1em' }}>
            Глобальный тест (GT): {data.globalTestValue.toFixed(5)}
          </span>
          <span style={{ fontWeight: 'bold' }}>
            {isGtPassed ? '✅ ТЕСТ ПРОЙДЕН' : '❌ ГРУБАЯ ОШИБКА'}
          </span>
        </div>
        <p style={{ margin: '5px 0 0 0', fontSize: '0.9em', opacity: 0.9 }}>
          {isGtPassed
            ? "Статистических оснований полагать наличие грубых ошибок нет."
            : "Внимание! Значение превышает порог Хи-квадрат. В данных измерений присутствует грубая ошибка."}
        </p>
      </div>

      {!isGtPassed && data.grossErrorStreams && data.grossErrorStreams.length > 0 && (
        <div style={{
          marginBottom: '20px',
          padding: '12px',
          backgroundColor: '#fff1f2',
          border: '1px solid #e11d48',
          borderRadius: '6px'
        }}>
          <strong style={{ color: 'e11d48', display: 'block', marginBottom: '5px' }}>
            Рекомендуются к проверке следующие датчики:
          </strong>
          <ul style={{ margin: 0, paddingLeft: '20px', color: '#991b1b' }}>
            {data.grossErrorStreams.map(streamId => {
              const stream = data.reconciledVariables.find(v => v.id === streamId);
              return <li key={streamId}>{stream?.name || streamId}</li>;
            })}
          </ul>
        </div>
      )}

      <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%', textAlign: 'left' }}>
        <thead style={{ backgroundColor: '#f8f9fa' }}>
          <tr>
            <th>Имя</th>
            <th>Было (Измерено)</th>
            <th>Стало (Согласовано)</th>
            <th>Поправка (Δ)</th>
            <th>В % от изм.</th>
          </tr>
        </thead>
        <tbody>
          {data.reconciledVariables.map(v => {
            const wasValue = v.measured ?? (v as any).Measured ?? 0;
            const isMeasured = v.isMeasured ?? (v as any).IsMeasured ?? false;
            const becameValue = v.reconciledValue ?? (v as any).ReconciledValue ?? 0;

            const diff = becameValue - wasValue;

            // Расчет процента (защита от деления на ноль)
            const percentDiff = wasValue !== 0 ? (diff / wasValue) * 100 : 0;

            const isGrossError = data.grossErrorStreams?.includes(v.id) || v.isGrossError;

            return (
              <tr
                key={v.id || (v as any).Id}
                style={{ borderBottom: '1px solid #dee2e6', backgroundColor: isGrossError ? '#fee2e2' : 'transparent', transition: 'background-color 0.3s' }}
              >
                <td style={{ padding: '12px' }}>
                  <strong>{v.name || (v as any).Name}</strong>
                  {isGrossError && <span style={{ color: 'red', marginLeft: '5px' }}>⚠️ Грубая ошибка</span>}
                </td>
                <td style={{ padding: '8px' }}>
                  {isMeasured ? wasValue.toFixed(3) : <span style={{ color: '#999' }}>—</span>}
                </td>
                <td style={{ padding: '8px', fontWeight: 'bold' }}>
                  {becameValue.toFixed(3)}
                </td>
                <td style={{
                  color: diff > 0 ? '#28a745' : diff < 0 ? '#dc3545' : '#333',
                  fontWeight: 'bold'
                }}>
                  {diff > 0 ? `+${diff.toFixed(3)}` : diff.toFixed(3)}
                </td>
                <td style={{ fontSize: '0.9em', color: '#666' }}>
                  {percentDiff.toFixed(2)}%
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>

      {!isSuccess && (
        <p style={{ color: '#dc3545', marginTop: '10px' }}>
          ⚠️ Внимание: Математическая модель не смогла свести баланс в пределах заданных границ.
        </p>
      )}
    </div>
  );
};