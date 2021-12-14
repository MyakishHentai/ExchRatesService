-- Table: public.Codes

-- DROP TABLE IF EXISTS public."Codes";

CREATE TABLE IF NOT EXISTS public."Codes"
(
    "Id" character varying(8) COLLATE pg_catalog."default" NOT NULL,
    "Name" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "EngName" character varying(255) COLLATE pg_catalog."default",
    "Nominal" integer NOT NULL DEFAULT 1,
    "ParentCode" character varying(8) COLLATE pg_catalog."default",
    "NumCode" smallint,
    "CharCode" character(3) COLLATE pg_catalog."default",
    CONSTRAINT "Codes_pkey" PRIMARY KEY ("Id")
    --CONSTRAINT "UNIQUE_CharCode" UNIQUE ("CharCode"),
    --CONSTRAINT "UNIQUE_NumCode" UNIQUE ("NumCode")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Codes"
    OWNER to postgres;

-- Table: public.Quotes

-- DROP TABLE IF EXISTS public."Quotes";

CREATE TABLE IF NOT EXISTS public."Quotes"
(
    "Id" bigint GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
    "Date" date NOT NULL,
    "Name" character varying(255) COLLATE pg_catalog."default",
    CONSTRAINT "Quotes_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "UNIQUE_Date" UNIQUE ("Date")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Quotes"
    OWNER to postgres;

-- Table: public.CodeQuotes

-- DROP TABLE IF EXISTS public."CodeQuotes";

CREATE TABLE IF NOT EXISTS public."CodeQuotes"
(
    "QuoteId" bigint NOT NULL,
    "CodeId" character varying(8) COLLATE pg_catalog."default" NOT NULL,
    "Value" real DEFAULT 0.00,
    CONSTRAINT "CodeQuotes_pkey" PRIMARY KEY ("QuoteId", "CodeId"),
    CONSTRAINT "FK_Code" FOREIGN KEY ("CodeId")
        REFERENCES public."Codes" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT "FK_Quote" FOREIGN KEY ("QuoteId")
        REFERENCES public."Quotes" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."CodeQuotes"
    OWNER to postgres;