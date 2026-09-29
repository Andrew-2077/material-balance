import type { Variable, BalanceResponse } from '../types';

export const solveBalance = async (variables: Variable[]): Promise<BalanceResponse> => {
  const response = await fetch(`/api/balance/solve`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ variables }),
  });

  if (!response.ok) {
    throw new Error(`Ошибка сервера: ${response.statusText}`);
  }

  return response.json();
};