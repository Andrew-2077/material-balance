import React, { useState } from 'react';
import type { Variable } from '../types';

interface Props {
  onAdd: (variable: Variable) => void;
}

export const AddVariableForm: React.FC<Props> = ({ onAdd }) => {
  const [formData, setFormData] = useState<Partial<Variable>>({
    name: '', sourceId: '', destinationId: '', measured: 0, tolerance: 1, minBound: 0, maxBound: 1000
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.name) return alert('Имя потока обязательно');

    onAdd({
      id: crypto.randomUUID(),
      name: formData.name,
      sourceId: formData.sourceId || null,
      destinationId: formData.destinationId || null,
      measured: Number(formData.measured),
      tolerance: Number(formData.tolerance),
      isMeasured: true,
      minBound: Number(formData.minBound),
      maxBound: Number(formData.maxBound),
    });
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: 'flex', gap: '10px', flexWrap: 'wrap', marginBottom: '20px', padding: '15px', border: '1px solid #ddd' }}>
      <div><label>Имя: </label><input type="text" value={formData.name} onChange={e => setFormData({ ...formData, name: e.target.value })} required /></div>
      <div><label>Источник (ID): </label><input type="text" value={formData.sourceId || ''} onChange={e => setFormData({ ...formData, sourceId: e.target.value })} /></div>
      <div><label>Приемник (ID): </label><input type="text" value={formData.destinationId || ''} onChange={e => setFormData({ ...formData, destinationId: e.target.value })} /></div>
      <div><label>Измерение: </label><input type="number" step="0.001" value={formData.measured} onChange={e => setFormData({ ...formData, measured: Number(e.target.value) })} /></div>
      <div><label>Погрешность: </label><input type="number" step="0.001" value={formData.tolerance} onChange={e => setFormData({ ...formData, tolerance: Number(e.target.value) })} /></div>
      <button type="submit">Добавить поток</button>
    </form>
  );
};