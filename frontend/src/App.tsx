// import React, { useState } from 'react';
// import type { Variable, BalanceResponse } from './types';
// import { solveBalance } from './api/balanceApi';
// import { AddVariableForm } from './components/AddVariableForm';
// import { VariablesTable } from './components/VariablesTable';
// import { ResultsTable } from './components/ResultsTable';
// import { FlowChart } from './components/FlowChart';
// import { useEffect } from 'react';

// export const App: React.FC = () => {
//   const [variables, setVariables] = useState<Variable[]>([]);
//   const [results, setResults] = useState<BalanceResponse | null>(null);
//   const [isLoading, setIsLoading] = useState(false);
//   const [error, setError] = useState<string | null>(null);

//   useEffect(() => {
//     const saved = localStorage.getItem('balance_variables');
//     if (saved) {
//       try {
//         setVariables(JSON.parse(saved));
//       } catch (e) {
//         console.error("Ошибка загрузки из LocalStorage", e);
//       }
//     }
//   }, []);

//   // 2. Сохранение в LocalStorage при каждом изменении списка
//   useEffect(() => {
//     localStorage.setItem('balance_variables', JSON.stringify(variables));
//   }, [variables]);

//   const handleSolve = async () => {
//     if (variables.length === 0) return setError('Добавьте хотя бы один поток.');
//     setIsLoading(true); setError(null);
//     try {
//       const data = await solveBalance(variables);
//       setResults(data);
//     } catch (err: any) {
//       setError(err.message);
//     } finally {
//       setIsLoading(false);
//     }
//   };

//   const exportToJson = () => {
//     const dataStr = JSON.stringify(variables, null, 2);
//     const dataUri = 'data:application/json;charset=utf-8,' + encodeURIComponent(dataStr);

//     const exportFileDefaultName = 'balance-schema.json';

//     const linkElement = document.createElement('a');
//     linkElement.setAttribute('href', dataUri);
//     linkElement.setAttribute('download', exportFileDefaultName);
//     linkElement.click();
//   };

//   const importFromJson = (e: React.ChangeEvent<HTMLInputElement>) => {
//     const fileReader = new FileReader();
//     const file = e.target.files?.[0];

//     if (!file) return;

//     fileReader.onload = (event) => {
//       try {
//         const result = event.target?.result as string;
//         const importedVariables = JSON.parse(result);

//         // Простая проверка: является ли файл массивом
//         if (Array.isArray(importedVariables)) {
//           setVariables(importedVariables);
//           setResults(null); // Сбрасываем старые результаты расчета
//           alert('Схема успешно загружена!');
//         } else {
//           alert('Ошибка: Неверный формат файла JSON');
//         }
//       } catch (error) {
//         alert('Ошибка при чтении файла: ' + error);
//       }
//     };

//     fileReader.readAsText(file);
//     // Очищаем input, чтобы можно было загрузить тот же файл повторно
//     e.target.value = "";
//   };

//   return (
//     <div style={{ maxWidth: '1200px', margin: '0 auto', padding: '20px', fontFamily: 'sans-serif' }}>
//       <h1>Сведение материального баланса</h1>

//       <div style={{ marginBottom: '20px', display: 'flex', gap: '10px' }}>
//         <button
//           onClick={exportToJson}
//           style={{ padding: '8px 16px', cursor: 'pointer', borderRadius: '4px', border: '1px solid #ccc' }}
//         >
//           💾 Сохранить схему (JSON)
//         </button>

//         <label style={{
//           padding: '8px 16px',
//           cursor: 'pointer',
//           borderRadius: '4px',
//           border: '1px solid #ccc',
//           background: '#efefef'
//         }}>
//           📂 Загрузить схему (JSON)
//           <input
//             type="file"
//             accept=".json"
//             onChange={importFromJson}
//             style={{ display: 'none' }}
//           />
//         </label>
//       </div>

//       {/* Форма добавления всегда сверху */}
//       <AddVariableForm onAdd={v => { setVariables([...variables, v]); setResults(null); }} />

//       {/* Контейнер для таблицы и графа */}
//       <div style={{
//         display: 'grid',
//         gridTemplateColumns: '1fr 1fr', // Две колонки равной ширины
//         gap: '20px',
//         alignItems: 'start',
//         marginBottom: '20px'
//       }}>

//         {/* Левая колонка: Таблица */}
//         <div>
//           <h3>Список потоков</h3>
//           <VariablesTable
//             variables={variables}
//             onRemove={id => setVariables(variables.filter(v => v.id !== id))}
//           />
//         </div>

//         {/* Правая колонка: Граф */}
//         <div>
//           <h3>Визуализация схемы</h3>
//           <FlowChart variables={variables} results={results} />
//         </div>

//       </div>

//       {/* Кнопка расчета под основными блоками */}
//       <div style={{ display: 'flex', gap: '15px', alignItems: 'center', marginBottom: '20px' }}>
//         <button
//           onClick={handleSolve}
//           disabled={isLoading}
//           style={{ padding: '10px 20px', background: '#007bff', color: '#fff', border: 'none', cursor: 'pointer', borderRadius: '4px' }}
//         >
//           {isLoading ? 'Вычисление...' : 'Рассчитать баланс'}
//         </button>
//         {error && <span style={{ color: 'red' }}>{error}</span>}
//       </div>

//       {/* Таблица с результатами появляется в самом низу после расчета */}
//       <ResultsTable data={results} originalVariables={variables} />
//     </div>
//   );
// };

// export default App;

import { useState, useEffect } from "react";
import { PeriodSelector } from "./components/PeriodSelector";
import { PlantModelView } from "./components/PlantModelView";
import { ResultsTable } from './components/ResultsTable';
import type { Variable, BalanceResponse } from './types';
import { PlantModelGraph } from "./components/PlantModelGraph";

export const App = () => {
  const [plantModel, setPlantModel] = useState<Variable[]>([]);
  const [selectedPeriodId, setSelectedPeriodId] = useState<number | null>(null);
  const [results, setResults] = useState<BalanceResponse | null>(null);
  const [loading, setLoading] = useState(false);

  // Загрузка модели завода при старте страницы
  useEffect(() => {
    fetch(`/api/PlantOperations/model`)
      .then(res => res.json())
      .then(data => setPlantModel(data));
  }, []);

  // Расчет для выбранного периода
  const handleCalculate = async () => {
    if (!selectedPeriodId) return;

    setLoading(true);
    try {
      const response = await fetch(`/api/PlantOperations/calculate/${selectedPeriodId}`, {
        method: 'POST'
      });
      const data = await response.json();
      setResults(data);
    } catch (error) {
      console.error("Ошибка расчета: ", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ padding: '20px', maxWidth: '1000px', margin: '0 auto' }}>
      <h1>Сведение материального баланса</h1>

      {/* Модель завода */}
      <PlantModelView model={plantModel} />

      {/* Период */}
      <PeriodSelector onPeriodSelect={setSelectedPeriodId} />

      <button
        onClick={handleCalculate}
        disabled={!selectedPeriodId || loading}
        style={{ padding: '10px 20px', fontSize: '16px', cursor: 'pointer' }}
      >
        {loading ? 'Считаем...' : 'Запустить расчет баланса'}
      </button>

      {/* Графическая схема завода */}
      <PlantModelGraph
        originalVariables={plantModel}
        results={results}
      />

      {/* Результаты */}
      {results && (
        <ResultsTable
          data={results}
        />
      )}
    </div>
  );
};

export default App;