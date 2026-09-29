import React, { useEffect, useState } from "react";
import type { Period } from "../types";

interface Props {
    onPeriodSelect: (periodId: number) => void;
}

export const PeriodSelector: React.FC<Props> = ({ onPeriodSelect }) => {
    const [periods, setPeriods] = useState<Period[]>([]);

    useEffect(() => {
        // Загрузка списка периодов
        fetch(`/api/Periods`)
            .then(res => res.json())
            .then(data => setPeriods(data));
    }, []);

    return (
        <div style={{ marginBottom: '20px', padding: '10px', backgroundColor: '#f0f2f5', borderRadius: '8px' }}>
            <label style={{ marginRight: '10px', fontWeight: 'bold' }}>Выберите период: </label>
            <select onChange={(e) => onPeriodSelect(Number(e.target.value))} defaultValue="">
                <option value="" disabled>-- Выберите смену/время --</option>
                {periods.map(p => (
                    <option key={p.id} value={p.id}>{p.name} ({new Date(p.timestamp).toLocaleString()})</option>
                ))}
            </select>
        </div>
    );
};