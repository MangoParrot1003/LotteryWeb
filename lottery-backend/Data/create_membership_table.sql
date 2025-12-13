-- 创建会员记录表
CREATE TABLE IF NOT EXISTS membership_records (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id TEXT NOT NULL,
    membership_type TEXT NOT NULL,
    order_no TEXT NOT NULL UNIQUE,
    amount REAL NOT NULL,
    remaining_exports INTEGER NOT NULL,
    purchase_time TEXT NOT NULL,
    expiry_time TEXT,
    status TEXT NOT NULL DEFAULT 'active'
);

-- 创建索引
CREATE INDEX IF NOT EXISTS idx_membership_user ON membership_records(user_id);
CREATE INDEX IF NOT EXISTS idx_membership_order ON membership_records(order_no);
CREATE INDEX IF NOT EXISTS idx_membership_status ON membership_records(status);
