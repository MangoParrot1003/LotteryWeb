<template>
  <div class="multi-prize-draw">
    <h2 class="title">🎁 多奖项抽奖</h2>

    <!-- 奖项配置区 -->
    <div class="prize-config">
      <div class="prize-list">
        <div 
          v-for="(prize, index) in prizes" 
          :key="index"
          class="prize-item"
        >
          <div class="prize-number">{{ index + 1 }}</div>
          <input
            v-model="prize.prizeName"
            type="text"
            placeholder="奖项名称（如：一等奖）"
            class="prize-name-input"
          />
          <input
            v-model.number="prize.winnerCount"
            type="number"
            min="1"
            max="20"
            placeholder="人数"
            class="winner-count-input"
          />
          <button 
            @click="removePrize(index)"
            class="remove-btn"
            :disabled="prizes.length === 1"
          >
            ❌
          </button>
        </div>
      </div>

      <button @click="addPrize" class="add-prize-btn">
        ➕ 添加奖项
      </button>
    </div>

    <!-- 抽奖按钮 -->
    <button 
      @click="performDraw"
      class="draw-button"
      :disabled="isDrawing || !canDraw"
      :class="{ drawing: isDrawing }"
    >
      <span v-if="!isDrawing">🎲 开始抽奖</span>
      <span v-else>🎲 抽奖中...</span>
    </button>

    <!-- 抽奖结果展示 -->
    <div v-if="results.length > 0" class="results-section">
      <h3 class="results-title">🎉 抽奖结果</h3>
      <div class="results-grid">
        <div 
          v-for="(result, index) in results" 
          :key="index"
          class="result-card"
          :class="{ 'is-drawing': currentDrawingIndex === index }"
        >
          <div class="prize-header">
            <span class="prize-badge">{{ result.prizeName }}</span>
            <span class="winner-count-badge">{{ result.winners.length }} 人</span>
          </div>
          
          <div class="winners-list">
            <div 
              v-for="(winner, wIndex) in result.winners" 
              :key="winner.id"
              class="winner-item"
              :class="{ 'is-latest': isLatestWinner(index, wIndex) }"
            >
              <div class="winner-avatar">{{ winner.name.charAt(0) }}</div>
              <div class="winner-info">
                <div class="winner-name">{{ winner.name }}</div>
                <div class="winner-details">{{ winner.studentId }} | {{ winner.class }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 空状态 -->
    <div v-if="results.length === 0 && !isDrawing" class="empty-state">
      <div class="empty-icon">🎯</div>
      <p>配置奖项后点击开始抽奖</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import type { Student } from '../types/student';
import * as lotteryApi from '../api/lottery';

interface Prize {
  prizeName: string;
  winnerCount: number;
}

interface PrizeResult {
  prizeName: string;
  winners: Student[];
}

const props = defineProps<{
  filterGender?: string;
  filterClass?: string;
}>();

// 奖项配置
const prizes = ref<Prize[]>([
  { prizeName: '一等奖', winnerCount: 1 },
  { prizeName: '二等奖', winnerCount: 2 },
  { prizeName: '三等奖', winnerCount: 3 }
]);

// 抽奖状态
const isDrawing = ref(false);
const results = ref<PrizeResult[]>([]);
const currentDrawingIndex = ref(-1);
const currentWinnerIndex = ref(-1);

// 计算属性
const canDraw = computed(() => {
  return prizes.value.every(p => p.prizeName.trim() && p.winnerCount > 0);
});

// 添加奖项
function addPrize() {
  prizes.value.push({
    prizeName: '',
    winnerCount: 1
  });
}

// 删除奖项
function removePrize(index: number) {
  if (prizes.value.length > 1) {
    prizes.value.splice(index, 1);
  }
}

// 判断是否是最新中奖者
function isLatestWinner(prizeIndex: number, winnerIndex: number) {
  return isDrawing.value && 
         prizeIndex === currentDrawingIndex.value && 
         winnerIndex === currentWinnerIndex.value;
}

// 执行抽奖
async function performDraw() {
  if (isDrawing.value || !canDraw.value) return;

  try {
    isDrawing.value = true;
    results.value = [];
    currentDrawingIndex.value = -1;
    currentWinnerIndex.value = -1;

    // 依次抽取每个奖项
    for (let i = 0; i < prizes.value.length; i++) {
      const prize = prizes.value[i]!;
      currentDrawingIndex.value = i;
      
      const winners: Student[] = [];
      
      // 依次抽取每个中奖者
      for (let j = 0; j < prize.winnerCount; j++) {
        currentWinnerIndex.value = j;
        
        // 动画效果
        await new Promise(resolve => setTimeout(resolve, 800));
        
        // 抽取一个学生
        const winner = await lotteryApi.drawStudent(
          props.filterGender,
          props.filterClass
        );
        
        winners.push(winner);
      }
      
      // 添加到结果
      results.value.push({
        prizeName: prize.prizeName,
        winners
      });
      
      // 奖项之间的间隔
      if (i < prizes.value.length - 1) {
        await new Promise(resolve => setTimeout(resolve, 1000));
      }
    }

    // 保存到后端
    await saveBatchPrizeDraw();
    
  } catch (error) {
    console.error('抽奖失败:', error);
    alert('抽奖失败，请重试');
  } finally {
    isDrawing.value = false;
    currentDrawingIndex.value = -1;
    currentWinnerIndex.value = -1;
  }
}

// 保存批量抽奖结果
async function saveBatchPrizeDraw() {
  try {
    const response = await fetch(`http://${window.location.hostname}:8502/api/lottery/prize-draw-batch`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        prizes: prizes.value,
        gender: props.filterGender,
        className: props.filterClass
      }),
    });

    if (!response.ok) {
      throw new Error('保存抽奖结果失败');
    }
  } catch (error) {
    console.error('保存抽奖结果失败:', error);
  }
}

// 清空结果
function clearResults() {
  results.value = [];
}

defineExpose({
  clearResults
});
</script>

<style scoped>
.multi-prize-draw {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
  background: linear-gradient(135deg, #fef3c7 0%, #fde68a 50%, #fef3c7 100%);
  border-radius: 24px;
  box-shadow: 0 8px 32px rgba(217, 119, 6, 0.3);
}

.title {
  text-align: center;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 50%, #b45309 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  font-size: 2.5rem;
  font-weight: 800;
  margin-bottom: 2rem;
  text-shadow: 2px 2px 4px rgba(217, 119, 6, 0.2);
  filter: drop-shadow(0 2px 4px rgba(217, 119, 6, 0.3));
}

/* 奖项配置区 */
.prize-config {
  background: linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%);
  border-radius: 20px;
  padding: 2rem;
  box-shadow: 0 8px 24px rgba(217, 119, 6, 0.2), inset 0 1px 0 rgba(255, 255, 255, 0.8);
  margin-bottom: 2rem;
  border: 2px solid #fbbf24;
}

.prize-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.prize-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem;
  background: linear-gradient(135deg, #ffffff 0%, #fef9e7 100%);
  border-radius: 12px;
  transition: all 0.3s;
  border: 2px solid #fde68a;
  box-shadow: 0 2px 8px rgba(217, 119, 6, 0.1);
}

.prize-item:hover {
  background: linear-gradient(135deg, #fef9e7 0%, #fef3c7 100%);
  border-color: #fbbf24;
  box-shadow: 0 4px 12px rgba(217, 119, 6, 0.2);
  transform: translateY(-2px);
}

.prize-number {
  width: 36px;
  height: 36px;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  flex-shrink: 0;
  box-shadow: 0 2px 8px rgba(217, 119, 6, 0.4), inset 0 1px 0 rgba(255, 255, 255, 0.3);
  font-size: 1.1rem;
}

.prize-name-input {
  flex: 1;
  padding: 0.8rem 1rem;
  font-size: 1rem;
  border: 2px solid #fbbf24;
  border-radius: 10px;
  transition: all 0.3s;
  background: white;
  box-shadow: inset 0 2px 4px rgba(217, 119, 6, 0.05);
}

.prize-name-input:focus {
  outline: none;
  border-color: #f59e0b;
  box-shadow: 0 0 0 3px rgba(245, 158, 11, 0.1), inset 0 2px 4px rgba(217, 119, 6, 0.05);
}

.winner-count-input {
  width: 100px;
  padding: 0.8rem 1rem;
  font-size: 1rem;
  border: 2px solid #fbbf24;
  border-radius: 10px;
  text-align: center;
  transition: all 0.3s;
  background: white;
  box-shadow: inset 0 2px 4px rgba(217, 119, 6, 0.05);
  font-weight: 600;
  color: #d97706;
}

.winner-count-input:focus {
  outline: none;
  border-color: #f59e0b;
  box-shadow: 0 0 0 3px rgba(245, 158, 11, 0.1), inset 0 2px 4px rgba(217, 119, 6, 0.05);
}

.remove-btn {
  padding: 0.5rem;
  background: transparent;
  border: none;
  cursor: pointer;
  font-size: 1.2rem;
  opacity: 0.6;
  transition: opacity 0.3s;
}

.remove-btn:hover:not(:disabled) {
  opacity: 1;
}

.remove-btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.add-prize-btn {
  width: 100%;
  padding: 1rem;
  font-size: 1rem;
  font-weight: 600;
  color: #d97706;
  background: linear-gradient(135deg, #ffffff 0%, #fef9e7 100%);
  border: 2px dashed #fbbf24;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 2px 8px rgba(217, 119, 6, 0.1);
}

.add-prize-btn:hover {
  background: linear-gradient(135deg, #fef9e7 0%, #fef3c7 100%);
  border-color: #f59e0b;
  color: #b45309;
  box-shadow: 0 4px 12px rgba(217, 119, 6, 0.2);
  transform: translateY(-2px);
}

/* 抽奖按钮 */
.draw-button {
  display: block;
  margin: 0 auto 2rem;
  padding: 1.5rem 4rem;
  font-size: 1.8rem;
  font-weight: 900;
  color: white;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 50%, #b45309 100%);
  border: none;
  border-radius: 60px;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 8px 24px rgba(217, 119, 6, 0.5), 
              inset 0 1px 0 rgba(255, 255, 255, 0.3),
              0 0 0 4px rgba(251, 191, 36, 0.3);
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
  position: relative;
  overflow: hidden;
}

.draw-button::before {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: linear-gradient(
    45deg,
    transparent,
    rgba(255, 255, 255, 0.3),
    transparent
  );
  transform: rotate(45deg);
  animation: shine 3s infinite;
}

@keyframes shine {
  0% {
    left: -50%;
  }
  100% {
    left: 150%;
  }
}

.draw-button:hover:not(:disabled) {
  transform: translateY(-4px) scale(1.05);
  box-shadow: 0 12px 32px rgba(217, 119, 6, 0.7), 
              inset 0 1px 0 rgba(255, 255, 255, 0.4),
              0 0 0 6px rgba(251, 191, 36, 0.4),
              0 0 40px rgba(245, 158, 11, 0.3);
}

.draw-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.draw-button.drawing {
  animation: goldenPulse 1s infinite, rotate 2s linear infinite;
}

@keyframes goldenPulse {
  0%, 100% {
    transform: scale(1);
    box-shadow: 0 8px 24px rgba(217, 119, 6, 0.5), 
                0 0 0 4px rgba(251, 191, 36, 0.3);
  }
  50% {
    transform: scale(1.08);
    box-shadow: 0 12px 32px rgba(217, 119, 6, 0.8), 
                0 0 0 8px rgba(251, 191, 36, 0.5),
                0 0 60px rgba(245, 158, 11, 0.5);
  }
}

@keyframes rotate {
  0% {
    filter: hue-rotate(0deg) brightness(1);
  }
  50% {
    filter: hue-rotate(10deg) brightness(1.2);
  }
  100% {
    filter: hue-rotate(0deg) brightness(1);
  }
}

/* 结果展示区 */
.results-section {
  margin-top: 2rem;
}

.results-title {
  text-align: center;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 50%, #b45309 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  font-size: 2rem;
  font-weight: 800;
  margin-bottom: 1.5rem;
  filter: drop-shadow(0 2px 4px rgba(217, 119, 6, 0.3));
}

.results-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
}

.result-card {
  background: linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%);
  border-radius: 20px;
  padding: 1.5rem;
  box-shadow: 0 8px 24px rgba(217, 119, 6, 0.2), 
              inset 0 1px 0 rgba(255, 255, 255, 0.8);
  transition: all 0.3s;
  border: 2px solid #fbbf24;
}

.result-card.is-drawing {
  animation: goldenCardGlow 1s ease-in-out infinite;
  transform: scale(1.03);
}

@keyframes goldenCardGlow {
  0%, 100% {
    box-shadow: 0 8px 24px rgba(217, 119, 6, 0.3), 
                inset 0 1px 0 rgba(255, 255, 255, 0.8);
    border-color: #fbbf24;
  }
  50% {
    box-shadow: 0 12px 36px rgba(217, 119, 6, 0.6), 
                0 0 40px rgba(245, 158, 11, 0.4),
                inset 0 1px 0 rgba(255, 255, 255, 1);
    border-color: #f59e0b;
  }
}

.prize-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f0f0f0;
}

.prize-badge {
  font-size: 1.5rem;
  font-weight: 900;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  filter: drop-shadow(0 2px 4px rgba(217, 119, 6, 0.3));
}

.winner-count-badge {
  padding: 0.4rem 1rem;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: 700;
  box-shadow: 0 2px 8px rgba(217, 119, 6, 0.4), 
              inset 0 1px 0 rgba(255, 255, 255, 0.3);
  text-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);
}

.winners-list {
  display: flex;
  flex-direction: column;
  gap: 0.8rem;
}

.winner-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.8rem;
  background: linear-gradient(135deg, #ffffff 0%, #fef9e7 100%);
  border-radius: 12px;
  transition: all 0.3s;
  border: 1px solid #fde68a;
}

.winner-item.is-latest {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  transform: scale(1.08);
  box-shadow: 0 8px 24px rgba(217, 119, 6, 0.6), 
              0 0 40px rgba(245, 158, 11, 0.4),
              inset 0 1px 0 rgba(255, 255, 255, 0.3);
  animation: goldenWinnerGlow 1s ease-in-out infinite;
  border: 2px solid #fbbf24;
}

@keyframes goldenWinnerGlow {
  0%, 100% {
    box-shadow: 0 8px 24px rgba(217, 119, 6, 0.6), 
                0 0 40px rgba(245, 158, 11, 0.4);
  }
  50% {
    box-shadow: 0 12px 32px rgba(217, 119, 6, 0.9), 
                0 0 60px rgba(245, 158, 11, 0.6),
                0 0 80px rgba(251, 191, 36, 0.3);
  }
}

.winner-item.is-latest .winner-avatar {
  background: linear-gradient(135deg, #ffffff 0%, #fef3c7 100%);
  color: #d97706;
  box-shadow: 0 4px 12px rgba(255, 255, 255, 0.5), 
              inset 0 2px 4px rgba(217, 119, 6, 0.2);
}

.winner-item.is-latest .winner-details {
  color: rgba(255, 255, 255, 0.9);
}

.winner-avatar {
  width: 48px;
  height: 48px;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2rem;
  font-weight: bold;
  flex-shrink: 0;
  box-shadow: 0 4px 12px rgba(217, 119, 6, 0.4), 
              inset 0 1px 0 rgba(255, 255, 255, 0.3);
  text-shadow: 0 1px 2px rgba(0, 0, 0, 0.2);
}

.winner-info {
  flex: 1;
}

.winner-name {
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 0.2rem;
}

.winner-item.is-latest .winner-name {
  color: white;
}

.winner-details {
  font-size: 0.9rem;
  color: #666;
}

/* 空状态 */
.empty-state {
  text-align: center;
  color: #999;
  padding: 3rem;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.empty-state p {
  font-size: 1.1rem;
}

/* 响应式 */
@media (max-width: 768px) {
  .results-grid {
    grid-template-columns: 1fr;
  }
  
  .prize-item {
    flex-wrap: wrap;
  }
  
  .prize-name-input {
    width: 100%;
  }
}
</style>
