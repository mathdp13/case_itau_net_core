export interface Fundo {
  codigo: string;
  nome: string;
  cnpj: string;
  codigoTipo: number;
  nomeTipo: string;
  patrimonio: number | null;
}

export interface CreateFundo {
  codigo: string;
  nome: string;
  cnpj: string;
  codigoTipo: number;
}

export interface UpdateFundo {
  nome: string;
  cnpj: string;
  codigoTipo: number;
}

export interface MovimentarPatrimonio {
  valor: number;
}

export interface TipoFundo {
  codigo: number;
  nome: string;
}
