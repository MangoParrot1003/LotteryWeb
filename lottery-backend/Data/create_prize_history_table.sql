-- 抽奖历史表
CREATE TABLE IF NOT EXISTS prize_draw_history (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    prize_name TEXT NOT NULL,
    winners TEXT NOT NULL,
    winner_count INTEGER NOT NULL,
    draw_time TEXT NOT NULL,
    session_id TEXT
);

-- 创建索引以提高查询性能
CREATE INDEX IF NOT EXISTS idx_prize_draw_history_draw_time ON prize_draw_history(draw_time DESC);
CREATE INDEX IF NOT EXISTS idx_prize_draw_history_session_id ON prize_draw_history(session_id);
