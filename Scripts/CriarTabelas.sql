

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/*Cria tabela de juros e multas*/
CREATE TABLE [dbo].[calcJuros] (
    [id]         INT        IDENTITY (1, 1) NOT NULL,
    [diasAtraso] INT        NOT NULL,
    [multa]      INT        NOT NULL,
    [jurosDia]   FLOAT (53) NOT NULL
);


/*Cria tabela de contas*/
CREATE TABLE [dbo].[contas] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [nome]           VARCHAR (60)    NOT NULL,
    [valorOriginal]  DECIMAL (15, 2) NOT NULL,
    [valorCorrigido] DECIMAL (15, 2) NOT NULL,
    [dataVencimento] DATETIME        NOT NULL,
    [dataPagamento]  DATETIME        NOT NULL,
    [quantDiaAtraso] INT             NOT NULL
);




/*Json para base de teste*/
--{
--        "nome":"Testee",
--        "valorOriginal":14.78,
--        "valorCorrigido": 0,
--        "dataVencimento":"2020-10-01",
--        "dataPagamento":"2020-10-20",
--        "quantDiaAtraso":0
--}