-- View: public.v_alltransactions

-- DROP VIEW public.v_alltransactions;

CREATE OR REPLACE VIEW public.v_alltransactions
 AS
 SELECT p.id AS trans_id,
    p.user_id,
    p.original_amount * '-1'::integer::numeric AS original_amount,
    COALESCE(c.name, 'Неизвестно'::text) AS currency_name,
    p.amount_in_tenge * '-1'::integer::numeric AS amount_in_tenge,
    p.comment,
    COALESCE(s.name, 'В обработке'::text) AS status_name,
    p.created_at,
    'Payment'::text AS trans_type
   FROM payment p
     LEFT JOIN currency c ON p.currency_id = c.id
     LEFT JOIN status s ON p.status_id = s.id
UNION ALL
 SELECT t.id AS trans_id,
    t.user_id,
    t.original_amount,
    COALESCE(c.name, 'Неизвестно'::text) AS currency_name,
    t.amount_in_tenge,
    t.comment,
    COALESCE(s.name, 'В обработке'::text) AS status_name,
    t.created_at,
    'TopUp'::text AS trans_type
   FROM topup t
     LEFT JOIN currency c ON t.currency_id = c.id
     LEFT JOIN status s ON t.status_id = s.id;

ALTER TABLE public.v_alltransactions
    OWNER TO postgres;

