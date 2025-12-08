<template>
  <div class="grouping-history-panel">
    <div class="panel-header">
      <h3>📋 分组历史</h3>
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
      <div v-if="groupedHistory.length === 0" class="empty-state">
        <p>暂无分组记录</p>
      </div>

      <div v-else class="history-list">
        <div 
          v-for="batch in groupedHistory" 
          :key="batch.batchId"
          class="batch-item"
        >
          <div class="batch-header">
            <div class="batch-info">
              <span class="batch-title">{{ batch.totalGroups }} 组 · {{ batch.groupSize }} 人/组</span>
              <span class="batch-time">{{ formatTime(batch.groupTime) }}</span>
            </div>
            <button 
              @click="$emit('remove', batch.batchId)" 
              class="remove-btn"
              title="删除此批次"
            >
              ×
            </button>
          </div>
          
          <div class="batch-groups">
            <div 
              v-for="group in batch.groups" 
              :key="group.groupNumber"
              class="mini-group"
            >
              <span class="group-label">第{{ group.groupNumber }}组</span>
              <div class="group-members-mini">
                <span 
                  v-for="member in group.members" 
                  :key="member.id"
                  class="member-tag"
                >
                  {{ member.name }}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="history.length > 0" class="panel-footer">
      <div class="history-count">
        共 {{ groupedHistory.length }} 次分组记录
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { GroupingHistory } from '../types/student';

const props = defineProps<{
  history: GroupingHistory[];
}>();

defineEmits<{
  'clear': [];
  'remove': [batchId: string];
}>();

// 将历史记录按批次分组
const groupedHistory = computed(() => {
  const batchMap = new Map<string, {
    batchId: string;
    groupTime: string;
    groupSize: number;
    totalGroups: number;
    groups: Array<{
      groupNumber: number;
      members: Array<{ id: number; name: string; studentId: string }>;
    }>;
  }>();

  for (const record of props.history) {
    if (!batchMap.has(record.batchId)) {
      batchMap.set(record.batchId, {
        batchId: record.batchId,
        groupTime: record.groupTime,
        groupSize: record.groupSize,
        totalGroups: record.totalGroups,
        groups: []
      });
    }
    
    try {
      const members = JSON.parse(record.members);
      batchMap.get(record.batchId)!.groups.push({
        groupNumber: record.groupNumber,
        members: members
      });
    } catch (e) {
      console.error('解析分组成员失败:', e);
    }
  }

  // 按时间排序，最新的在前
  return Array.from(batchMap.values())
    .sort((a, b) => new Date(b.groupTime || 0).getTime() - new Date(a.groupTime || 0).getTime());
});

// 格式化时间
function formatTime(dateStr: string | undefined): string {
  if (!dateStr) return '';
  const cleanDateStr = dateStr.split('.')[0] || dateStr;
  const date = new Date(cleanDateStr);
  
  if (isNaN(date.getTime())) {
    return dateStr;
  }
  
  const now = new Date();
  const diff = now.getTime() - date.getTime();
  
  if (diff < 60000 && diff >= 0) {
    return '刚刚';
  }
  
  if (diff < 3600000 && diff >= 0) {
    const minutes = Math.floor(diff / 60000);
    return `${minutes}分钟前`;
  }
  
  if (diff < 86400000 && diff >= 0) {
    const hours = Math.floor(diff / 3600000);
    return `${hours}小时前`;
  }
  
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const day = date.getDate().toString().padStart(2, '0');
  const hour = date.getHours().toString().padStart(2, '0');
  const minute = date.getMinutes().toString().padStart(2, '0');
  
  const isToday = date.toDateString() === now.toDateString();
  
  if (isToday) {
    return `今天 ${hour}:${minute}`;
  }
  
  return `${month}-${day} ${hour}:${minute}`;
}
</script>

<style scoped>
.grouping-history-panel {
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
  gap: 0.8rem;
}

.batch-item {
  background: #f8f9fa;
  border-radius: 10px;
  overflow: hidden;
  border-left: 3px solid #667eea;
}

.batch-header {
  padding: 0.8rem 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #f0f4ff;
}

.batch-info {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.batch-title {
  font-weight: 600;
  color: #333;
  font-size: 0.95rem;
}

.batch-time {
  font-size: 0.8rem;
  color: #666;
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

.batch-groups {
  padding: 0.8rem 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}

.mini-group {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.group-label {
  font-size: 0.8rem;
  color: #667eea;
  font-weight: 600;
}

.group-members-mini {
  display: flex;
  flex-wrap: wrap;
  gap: 0.3rem;
}

.member-tag {
  background: white;
  padding: 0.2rem 0.5rem;
  border-radius: 4px;
  font-size: 0.85rem;
  color: #333;
  border: 1px solid #e0e0e0;
}

.panel-footer {
  padding: 0.8rem 1.2rem;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
}

.history-count {
  color: #666;
  font-size: 0.9rem;
  font-weight: 500;
  text-align: center;
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
