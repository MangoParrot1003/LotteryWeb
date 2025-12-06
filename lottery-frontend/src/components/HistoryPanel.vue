<template>
  <div class="history-panel">
    <div class="panel-header">
      <h3>📜 抽签历史</h3>
      <button 
        v-if="history.length > 0" 
        @click="$emit('clear')" 
        class="clear-btn"
        title="清空历史"
      >
        🗑️ 清空
      </button>
    </div>

    <div class="history-content">
      <div v-if="history.length === 0" class="empty-state">
        <p>暂无抽签记录</p>
      </div>

      <div v-else class="history-list">
        <template v-for="(record, index) in history" :key="record.id">
          <!-- 批次分隔线 -->
          <div 
            v-if="index > 0 && shouldShowBatchSeparator(record, history[index - 1])"
            class="batch-separator"
          >
            <span class="separator-line"></span>
          </div>
          
          <!-- 历史记录项 -->
          <div class="history-item" :class="{ 'is-batch': record.isBatch }">
            <div class="item-header">
              <div class="item-number">{{ history.length - index }}</div>
              <div class="item-time">{{ formatTime(record.drawTime) }}</div>
            </div>
            <div class="item-body">
              <div class="item-info">
                <div class="item-name">
                  {{ record.studentName }}
                  <span v-if="record.isBatch" class="batch-badge">批量</span>
                </div>
                <div class="item-details">
                  {{ record.studentNumber }} | {{ record.class }}
                </div>
              </div>
              <button 
                @click="$emit('remove', record.id)" 
                class="remove-btn"
                title="移除"
              >
                ×
              </button>
            </div>
          </div>
        </template>
      </div>
    </div>

    <div v-if="history.length > 0" class="panel-footer">
      <label class="exclude-checkbox">
        <input 
          type="checkbox" 
          :checked="excludeDrawn"
          @change="$emit('update:excludeDrawn', ($event.target as HTMLInputElement).checked)"
        />
        <span>排除已抽取</span>
      </label>
      <div class="history-count">
        已抽取 {{ history.length }} 人
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { DrawHistory } from '../types/student';

defineProps<{
  history: DrawHistory[];
  excludeDrawn: boolean;
}>();

defineEmits<{
  'clear': [];
  'remove': [id: number];
  'update:excludeDrawn': [value: boolean];
}>();

// 格式化时间
function formatTime(dateStr: string): string {
  // 处理后端返回的时间格式（可能包含微秒）
  const cleanDateStr = dateStr.split('.')[0]; // 移除微秒部分
  const date = new Date(cleanDateStr);
  
  // 检查日期是否有效
  if (isNaN(date.getTime())) {
    return dateStr; // 如果解析失败，返回原始字符串
  }
  
  const now = new Date();
  const diff = now.getTime() - date.getTime();
  
  // 小于1分钟
  if (diff < 60000 && diff >= 0) {
    return '刚刚';
  }
  
  // 小于1小时
  if (diff < 3600000 && diff >= 0) {
    const minutes = Math.floor(diff / 60000);
    return `${minutes}分钟前`;
  }
  
  // 小于24小时
  if (diff < 86400000 && diff >= 0) {
    const hours = Math.floor(diff / 3600000);
    return `${hours}小时前`;
  }
  
  // 显示具体时间
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const day = date.getDate().toString().padStart(2, '0');
  const hour = date.getHours().toString().padStart(2, '0');
  const minute = date.getMinutes().toString().padStart(2, '0');
  const second = date.getSeconds().toString().padStart(2, '0');
  
  // 判断是否是今天
  const isToday = date.toDateString() === now.toDateString();
  
  if (isToday) {
    return `今天 ${hour}:${minute}:${second}`;
  }
  
  // 判断是否是今年
  const isThisYear = date.getFullYear() === now.getFullYear();
  
  if (isThisYear) {
    return `${month}-${day} ${hour}:${minute}:${second}`;
  }
  
  return `${date.getFullYear()}-${month}-${day} ${hour}:${minute}:${second}`;
}

// 判断是否显示批次分隔线
function shouldShowBatchSeparator(current: DrawHistory, previous: DrawHistory): boolean {
  // 如果当前是批量抽签，且与上一条的 batchId 不同
  if (current.isBatch && current.batchId !== previous.batchId) {
    return true;
  }
  
  // 如果上一条是批量抽签，当前不是
  if (previous.isBatch && !current.isBatch) {
    return true;
  }
  
  // 如果都不是批量抽签
  if (!current.isBatch && !previous.isBatch) {
    return true;
  }
  
  return false;
}
</script>

<style scoped>
.history-panel {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  max-height: 500px;
}

.panel-header {
  padding: 1rem 1.2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel-header h3 {
  margin: 0;
  font-size: 1.1rem;
}

.clear-btn {
  background: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: white;
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: all 0.2s;
}

.clear-btn:hover {
  background: rgba(255, 255, 255, 0.3);
}

.history-content {
  flex: 1;
  overflow-y: auto;
  padding: 0.5rem;
}

.empty-state {
  text-align: center;
  padding: 2rem;
  color: #999;
}

.history-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.batch-separator {
  margin: 0.8rem 0;
  display: flex;
  align-items: center;
}

.separator-line {
  flex: 1;
  height: 1px;
  background: linear-gradient(90deg, transparent, #ddd, transparent);
}

.history-item {
  padding: 0.8rem;
  background: #f8f9fa;
  border-radius: 8px;
  transition: all 0.2s;
}

.history-item.is-batch {
  background: #f0f4ff;
  border-left: 3px solid #667eea;
}

.history-item:hover {
  background: #e9ecef;
  transform: translateX(2px);
}

.history-item.is-batch:hover {
  background: #e6edff;
}

.item-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.item-number {
  width: 24px;
  height: 24px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.75rem;
  font-weight: bold;
  flex-shrink: 0;
}

.item-time {
  font-size: 0.75rem;
  color: #999;
}

.item-body {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}

.item-info {
  flex: 1;
  min-width: 0;
}

.item-name {
  font-weight: 600;
  color: #333;
  margin-bottom: 0.2rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.batch-badge {
  display: inline-block;
  padding: 0.1rem 0.4rem;
  background: #667eea;
  color: white;
  font-size: 0.7rem;
  border-radius: 4px;
  font-weight: normal;
}

.item-details {
  font-size: 0.85rem;
  color: #666;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.remove-btn {
  width: 24px;
  height: 24px;
  background: #dc3545;
  color: white;
  border: none;
  border-radius: 50%;
  cursor: pointer;
  font-size: 1.2rem;
  line-height: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.2s;
}

.remove-btn:hover {
  background: #c82333;
  transform: scale(1.1);
}

.panel-footer {
  padding: 1rem 1.2rem;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.exclude-checkbox {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  user-select: none;
}

.exclude-checkbox input[type="checkbox"] {
  width: 18px;
  height: 18px;
  cursor: pointer;
}

.exclude-checkbox span {
  color: #333;
  font-size: 0.95rem;
}

.history-count {
  color: #666;
  font-size: 0.9rem;
  font-weight: 500;
}

/* 滚动条样式 */
.history-content::-webkit-scrollbar {
  width: 6px;
}

.history-content::-webkit-scrollbar-track {
  background: #f1f1f1;
}

.history-content::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 3px;
}

.history-content::-webkit-scrollbar-thumb:hover {
  background: #555;
}
</style>
