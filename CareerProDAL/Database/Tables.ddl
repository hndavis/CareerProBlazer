-- Table: public.address

-- DROP TABLE public.address;

CREATE TABLE public.address
(
    address_id uuid NOT NULL,
    person_id uuid,
    address1 character(100) COLLATE pg_catalog."default",
    address2 character(100) COLLATE pg_catalog."default",
    city character(50) COLLATE pg_catalog."default",
    state character(4) COLLATE pg_catalog."default",
    zip character(10) COLLATE pg_catalog."default",
    CONSTRAINT address_pkey PRIMARY KEY (address_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.address
    OWNER to postgres;


-- Table: public.email

-- DROP TABLE public.email;

CREATE TABLE public.email
(
    email_id uuid NOT NULL,
    person_id uuid NOT NULL,
    email character(150) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT email_pkey PRIMARY KEY (email_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.person
    OWNER to postgres;


-- Table: public.person

-- DROP TABLE public.person;

CREATE TABLE public.person
(
    person_id uuid NOT NULL,
    first_name character(100) COLLATE pg_catalog."default",
    middle_name character(100) COLLATE pg_catalog."default",
    last_name character(100) COLLATE pg_catalog."default",
    CONSTRAINT person_pkey PRIMARY KEY (person_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.person
    OWNER to postgres;

    -- Table: public.phone

-- DROP TABLE public.phone;

CREATE TABLE public.phone
(
    phone_id uuid NOT NULL,
    person_id uuid NOT NULL,
    address_id uuid,
    "number" character(15) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT phone_pkey PRIMARY KEY (phone_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.phone
    OWNER to postgres;

    -- Table: public.resume

-- DROP TABLE public.resume;

CREATE TABLE public.resume
(
    resume_id uuid NOT NULL,
    resume_raw_id uuid NOT NULL,
    "desc" character(100) COLLATE pg_catalog."default",
    date timestamp with time zone,
    person_id uuid,
    CONSTRAINT resume_pkey PRIMARY KEY (resume_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.resume
    OWNER to postgres;

    -- Table: public.resumes_raw

-- DROP TABLE public.resumes_raw;

CREATE TABLE public.resumes_raw
(
    resume_id uuid NOT NULL,
    resume_name character(50) COLLATE pg_catalog."default" NOT NULL,
    date timestamp with time zone,
    resume_desc character(100) COLLATE pg_catalog."default",
    resume bytea,
    orig_file_name character(256) COLLATE pg_catalog."default",
    CONSTRAINT resumes_pkey PRIMARY KEY (resume_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.resumes_raw
    OWNER to postgres;