<template>
  <div class="prize-history-panel">
    <div class="panel-header">
      <h3>🎁 抽奖历史</h3>
      <button 
        v-if="history.length > 0" 
        @click="$emit('clear')"
        class="clear-btn"
        title="清空所有历史"
      >
        🗑️ 清空
      </button>
    </div>

    <div v-if="history.length === 0" class="empty-history">
      <div class="empty-icon">📋</div>
      <p>暂无抽奖记录</p>
    </div>

    <div v-else class="history-list">
      <div 
        v-for="item in history" 
        :key="item.id"
        class="history-item"
      >
        <div class="item-header">
          <div class="prize-name">🏆 {{ item.prizeName }}</div>
          <button 
            @click="$emit('remove', item.id)"
            class="delete-btn"
            title="删除此记录"
          >
            ×
          </button>
        </div>
        
        <div class="winners-list">
          <div 
            v-for="(winner, index) in getWinners(item)"
            :key="index"
            class="winner-chip"
          >
            <span class="winner-name">{{ winner.name }}</span>
            <span class="winner-info">({{ winner.studentId }})</span>
          </div>
        </div>
        
        <div class="item-footer">
          <span class="draw-time">{{ formatTime(item.drawTime) }}</span>
          <span class="winner-count">{{ item.winnerCount }}人中奖</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Winner {
  id: number;
  studentId: string;
  name: string;
  gender?: string;
  class?: string;
  major?: string;
}

interface PrizeHistory {
  id: number;
  prizeName: string;
  winners: Winner[];
  winnerCount: number;
  drawTime: string;
  sessionId?: string;
}

defineProps<{
  history: PrizeHistory[];
}>();

defineEmits<{
  'clear': [];
  'remove': [id: number];
}>();

// 获取中奖者数据（优先使用 winnersList，其次使用 winners）
function getWinners(item: any): Winner[] {
  // 优先使用 winnersList（后端已解析的数据）
  if (item.winnersList && Array.isArray(item.winnersList)) {
    return item.winnersList;
  }
  
  // 其次使用 winners（可能是字符串或数组）
  return parseWinners(item.winners);
}

// 解析中奖者数据（可能是字符串或数组）
function parseWinners(winners: any): Winner[] {
  if (!winners) return [];
  
  // 如果已经是数组，直接返回
  if (Array.isArray(winners)) {
    return winners;
  }
  
  // 如果是字符串，尝试解析 JSON
  if (typeof winners === 'string') {
    try {
      const parsed = JSON.parse(winners);
      return Array.isArray(parsed) ? parsed : [];
    } catch (e) {
      console.error('解析中奖者数据失败:', e);
      return [];
    }
  }
  
  return [];
}

function formatTime(timeStr: string): string {
  const date = new Date(timeStr);
  const now = new Date();
  const diffMs = now.getTime() - date.getTime();
  const diffMins = Math.floor(diffMs / 60000);
  
  if (diffMins < 1) return '刚刚';
  if (diffMins < 60) return `${diffMins}分钟前`;
  
  const diffHours = Math.floor(diffMins / 60);
  if (diffHours < 24) return `${diffHours}小时前`;
  
  const diffDays = Math.floor(diffHours / 24);
  if (diffDays < 7) return `${diffDays}天前`;
  
  return date.toLocaleDateString('zh-CN');
}
</script>

<style scoped>
.prize-history-panel {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  height: 100%;
  display: flex;
  flex-direction: column;
}

.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
  padding-bottom: 0.8rem;
  border-bottom: 2px solid #fef3c7;
}

.panel-header h3 {
  margin: 0;
  color: #d97706;
  font-size: 1.2rem;
}

.clear-btn {
  padding: 0.4rem 0.8rem;
  background: #fef3c7;
  color: #78350f;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: all 0.2s;
}

.clear-btn:hover {
  background: #fde68a;
  transform: translateY(-1px);
}

.empty-history {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #d1d5db;
  padding: 3rem 1rem;
}

.empty-icon {
  font-size: 3rem;
  margin-bottom: 0.5rem;
  opacity: 0.5;
}

.empty-history p {
  font-size: 1rem;
  margin: 0;
}

.history-list {
  flex: 1;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.history-item {
  background: linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%);
  border-radius: 8px;
  padding: 1rem;
  border: 1px solid #fde68a;
  transition: all 0.2s;
}

.history-item:hover {
  box-shadow: 0 4px 12px rgba(245, 158, 11, 0.15);
  transform: translateY(-2px);
}

.item-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.8rem;
}

.prize-name {
  font-weight: 600;
  color: #d97706;
  font-size: 1rem;
}

.delete-btn {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  border: none;
  background: rgba(217, 119, 6, 0.1);
  color: #d97706;
  cursor: pointer;
  font-size: 1.2rem;
  line-height: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

.delete-btn:hover {
  background: #d97706;
  color: white;
}

.winners-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-bottom: 0.8rem;
}

.winner-chip {
  background: white;
  padding: 0.3rem 0.6rem;
  border-radius: 12px;
  font-size: 0.85rem;
  color: #78350f;
  border: 1px solid #fde68a;
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
}

.winner-name {
  font-weight: 500;
}

.winner-info {
  color: #92400e;
  opacity: 0.7;
  font-size: 0.75rem;
}

.item-footer {
  display: flex;
  justify-content: space-between;
  font-size: 0.75rem;
  color: #92400e;
  opacity: 0.7;
  margin-top: 0.5rem;
  padding-top: 0.5rem;
  border-top: 1px solid rgba(253, 230, 138, 0.5);
}

.draw-time,
.winner-count {
  display: flex;
  align-items: center;
}

/* 滚动条样式 */
.history-list::-webkit-scrollbar {
  width: 6px;
}

.history-list::-webkit-scrollbar-track {
  background: #fef3c7;
  border-radius: 3px;
}

.history-list::-webkit-scrollbar-thumb {
  background: #fbbf24;
  border-radius: 3px;
}

.history-list::-webkit-scrollbar-thumb:hover {
  background: #f59e0b;
}
</style>
