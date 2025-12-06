-- 创建抽签历史记录表
CREATE TABLE IF NOT EXISTS draw_history (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    student_id INTEGER NOT NULL,
    student_name TEXT NOT NULL,
    student_number TEXT NOT NULL,
    class TEXT,
    gender TEXT,
    draw_time TEXT NOT NULL,
    session_id TEXT,
    is_batch INTEGER NOT NULL DEFAULT 0,
    batch_id TEXT,
    FOREIGN KEY (student_id) REFERENCES students(id)
);

-- 创建索引以提高查询性能
CREATE INDEX IF NOT EXISTS idx_draw_history_student_id ON draw_history(student_id);
CREATE INDEX IF NOT EXISTS idx_draw_history_draw_time ON draw_history(draw_time);
CREATE INDEX IF NOT EXISTS idx_draw_history_session_id ON draw_history(session_id);
CREATE INDEX IF NOT EXISTS idx_draw_history_batch_id ON draw_history(batch_id);
