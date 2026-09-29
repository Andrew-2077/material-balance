export interface Variable {
  id: string;
  name: string;
  sourceId: string | null;
  destinationId: string | null;
  measured: number;
  tolerance: number;
  isMeasured: boolean;
  minBound: number;
  maxBound: number;
}

export interface BalanceResult {
  id: string;
  name: string;
  reconciledValue: number;
  measured: number;
  isMeasured: boolean;
  isGrossError?: boolean;
}

export interface BalanceResponse {
  reconciledVariables: BalanceResult[];
  status: string;
  isGlobalTestPassed: boolean;
  globalTestValue: number;
  grossErrorStreams?: string[];
}

export interface Period {
  id: number;
  name: string;
  timestamp: string;
}