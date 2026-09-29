import React from "react";
import type { Variable } from "../types";

export const PlantModelView: React.FC<{ model: Variable[] }> = ({ model }) => {
    return (
        <details style={{ marginBottom: '20px', border: '1px solid #ccc', borderRadius: '4px' }}>
            <summary style={{ padding: '10px', cursor: 'pointer', backgroundColor: '#eee' }}>
                <strong>🔍 Показать структуру завода (статичная модель)</strong>
            </summary>
            <div style={{ padding: '15px' }}>
                <table border={1} style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr style={{ backgroundColor: '#fafafa' }}>
                            <th>Поток</th>
                            <th>Откуда (source)</th>
                            <th>Куда (destination)</th>
                            <th>Мин/Макс</th>
                        </tr>
                    </thead>
                    <tbody>
                        {model.map(v => (
                            <tr key={v.id}>
                                <td>{v.name}</td>
                                <td>{v.sourceId || '--- ВХОД ---'}</td>
                                <td>{v.destinationId || '--- ВЫХОД ---'}</td>
                                <td>[{v.minBound} ; {v.maxBound}]</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </details>
    );
};