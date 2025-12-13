<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-container">
      <!-- 关闭按钮 -->
      <button class="close-btn" @click="$emit('close')">×</button>

      <!-- 订单信息 -->
      <div class="order-info">
        <h2>💳 确认支付</h2>
        <div class="order-details">
          <div class="detail-row">
            <span class="label">套餐名称：</span>
            <span class="value">{{ plan.name }}</span>
          </div>
          <div class="detail-row">
            <span class="label">订单金额：</span>
            <span class="value price">¥{{ plan.price }}</span>
          </div>
          <div class="detail-row">
            <span class="label">订单编号：</span>
            <span class="value order-no">{{ orderNo }}</span>
          </div>
        </div>
      </div>

      <!-- 支付方式选择 -->
      <div class="payment-methods">
        <h3>选择支付方式</h3>
        <div class="method-buttons">
          <button 
            class="method-btn"
            :class="{ active: paymentMethod === 'wechat' }"
            @click="selectPaymentMethod('wechat')"
          >
            <span class="method-icon">💚</span>
            <span class="method-name">微信支付</span>
          </button>
          <button 
            class="method-btn"
            :class="{ active: paymentMethod === 'alipay' }"
            @click="selectPaymentMethod('alipay')"
          >
            <span class="method-icon">💙</span>
            <span class="method-name">支付宝</span>
          </button>
        </div>
      </div>

      <!-- 二维码展示区域 -->
      <div class="qrcode-section" v-if="paymentMethod">
        <div class="qrcode-container">
          <div class="qrcode-placeholder">
            <div class="qr-icon">📱</div>
            <p class="qr-hint">
              请使用{{ paymentMethod === 'wechat' ? '微信' : '支付宝' }}扫描二维码支付
            </p>
            <!-- 这里可以集成真实的二维码生成库 -->
            <div class="mock-qrcode">
              <div class="qr-grid">
                <div v-for="i in 81" :key="i" class="qr-cell" :class="{ filled: isQRCellFilled(i) }"></div>
              </div>
            </div>
            <p class="amount-display">¥{{ plan.price }}</p>
          </div>
        </div>

        <!-- 支付状态 -->
        <div class="payment-status">
          <div v-if="paymentStatus === 'pending'" class="status-pending">
            <div class="loading-spinner"></div>
            <p>等待支付中...</p>
          </div>
          <div v-else-if="paymentStatus === 'success'" class="status-success">
            <div class="success-icon">✓</div>
            <p>支付成功！</p>
          </div>
          <div v-else-if="paymentStatus === 'failed'" class="status-failed">
            <div class="failed-icon">✗</div>
            <p>支付失败，请重试</p>
          </div>
        </div>

        <!-- 操作按钮 -->
        <div class="action-buttons">
          <button class="action-btn cancel-btn" @click="$emit('close')">
            取消支付
          </button>
          <button class="action-btn simulate-btn" @click="simulatePayment">
            模拟支付成功
          </button>
        </div>
      </div>

      <!-- 支付提示 -->
      <div class="payment-tips">
        <p>💡 提示：这是演示版本，点击"模拟支付成功"即可体验功能</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';

interface Plan {
  type: string;
  name: string;
  price: number;
}

const props = defineProps<{
  plan: Plan;
}>();

const emit = defineEmits<{
  close: [];
  paymentSuccess: [];
}>();

// 支付方式
const paymentMethod = ref<'wechat' | 'alipay' | null>(null);

// 支付状态
const paymentStatus = ref<'pending' | 'success' | 'failed'>('pending');

// 订单号
const orderNo = ref('');

// 生成订单号
const generateOrderNo = () => {
  const timestamp = Date.now();
  const random = Math.floor(Math.random() * 10000);
  return `ORD${timestamp}${random}`;
};

// 选择支付方式
const selectPaymentMethod = (method: 'wechat' | 'alipay') => {
  paymentMethod.value = method;
  paymentStatus.value = 'pending';
};

// 模拟二维码图案（简单的随机填充）
const isQRCellFilled = (index: number) => {
  // 创建一个伪随机模式
  const pattern = [1, 2, 3, 7, 8, 9, 10, 14, 15, 16, 19, 21, 23, 28, 30, 32, 37, 39, 41, 46, 48, 50, 55, 56, 57, 61, 62, 63, 64, 68, 69, 70, 73, 75, 77];
  return pattern.includes(index);
};

// 模拟支付成功
const simulatePayment = async () => {
  paymentStatus.value = 'success';
  
  // 调用后端创建会员
  try {
    const userId = 'default_user'; // 实际应用中应从登录状态获取
    const response = await fetch(`http://${window.location.hostname}:8502/api/lottery/membership`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        userId: userId,
        membershipType: props.plan.type,
        orderNo: orderNo.value,
        amount: props.plan.price
      })
    });
    
    if (response.ok) {
      // 保存会员信息到localStorage
      const data = await response.json();
      localStorage.setItem('membership', JSON.stringify(data.membership));
      
      setTimeout(() => {
        emit('paymentSuccess');
      }, 1500);
    }
  } catch (error) {
    console.error('创建会员失败:', error);
    // 即使API失败也继续流程（演示模式）
    setTimeout(() => {
      emit('paymentSuccess');
    }, 1500);
  }
};

// 组件挂载时生成订单号
onMounted(() => {
  orderNo.value = generateOrderNo();
});
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  animation: fadeIn 0.3s;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.modal-container {
  background: white;
  border-radius: 20px;
  padding: 2.5rem;
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
  position: relative;
  animation: slideUp 0.3s;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
}

@keyframes slideUp {
  from {
    transform: translateY(50px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.close-btn {
  position: absolute;
  top: 1rem;
  right: 1rem;
  width: 40px;
  height: 40px;
  border: none;
  background: #f3f4f6;
  border-radius: 50%;
  font-size: 1.5rem;
  color: #666;
  cursor: pointer;
  transition: all 0.3s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.close-btn:hover {
  background: #e5e7eb;
  transform: rotate(90deg);
}

/* 订单信息 */
.order-info {
  margin-bottom: 2rem;
}

.order-info h2 {
  color: #333;
  margin-bottom: 1.5rem;
  text-align: center;
}

.order-details {
  background: #f9fafb;
  border-radius: 12px;
  padding: 1.5rem;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  padding: 0.7rem 0;
  border-bottom: 1px solid #e5e7eb;
}

.detail-row:last-child {
  border-bottom: none;
}

.label {
  color: #6b7280;
  font-size: 0.95rem;
}

.value {
  color: #111827;
  font-weight: 500;
}

.value.price {
  color: #ef4444;
  font-size: 1.3rem;
  font-weight: bold;
}

.value.order-no {
  font-family: monospace;
  font-size: 0.9rem;
}

/* 支付方式 */
.payment-methods {
  margin-bottom: 2rem;
}

.payment-methods h3 {
  color: #333;
  font-size: 1.1rem;
  margin-bottom: 1rem;
}

.method-buttons {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.method-btn {
  padding: 1.2rem;
  border: 2px solid #e5e7eb;
  background: white;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.3s;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
}

.method-btn:hover {
  border-color: #667eea;
  transform: translateY(-2px);
}

.method-btn.active {
  border-color: #667eea;
  background: linear-gradient(135deg, rgba(102, 126, 234, 0.1) 0%, rgba(118, 75, 162, 0.1) 100%);
}

.method-icon {
  font-size: 2rem;
}

.method-name {
  font-size: 1rem;
  font-weight: 500;
  color: #333;
}

/* 二维码区域 */
.qrcode-section {
  margin-bottom: 2rem;
}

.qrcode-container {
  background: #f9fafb;
  border-radius: 12px;
  padding: 2rem;
  text-align: center;
}

.qr-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.qr-hint {
  color: #6b7280;
  margin-bottom: 1.5rem;
}

.mock-qrcode {
  display: inline-block;
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  margin-bottom: 1rem;
}

.qr-grid {
  display: grid;
  grid-template-columns: repeat(9, 1fr);
  gap: 3px;
  width: 180px;
  height: 180px;
}

.qr-cell {
  background: white;
  border-radius: 1px;
}

.qr-cell.filled {
  background: #111827;
}

.amount-display {
  font-size: 1.5rem;
  font-weight: bold;
  color: #ef4444;
  margin: 1rem 0 0 0;
}

/* 支付状态 */
.payment-status {
  margin-bottom: 1.5rem;
  text-align: center;
}

.status-pending,
.status-success,
.status-failed {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  padding: 1rem;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f4f6;
  border-top-color: #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.success-icon,
.failed-icon {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: bold;
}

.success-icon {
  background: #10b981;
  color: white;
}

.failed-icon {
  background: #ef4444;
  color: white;
}

/* 操作按钮 */
.action-buttons {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1rem;
}

.action-btn {
  padding: 0.9rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s;
}

.cancel-btn {
  background: #f3f4f6;
  color: #6b7280;
}

.cancel-btn:hover {
  background: #e5e7eb;
}

.simulate-btn {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  color: white;
}

.simulate-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(16, 185, 129, 0.4);
}

/* 支付提示 */
.payment-tips {
  background: #fef3c7;
  border-left: 4px solid #f59e0b;
  padding: 1rem;
  border-radius: 8px;
}

.payment-tips p {
  margin: 0;
  color: #92400e;
  font-size: 0.9rem;
}

/* 响应式设计 */
@media (max-width: 600px) {
  .modal-container {
    padding: 1.5rem;
  }

  .method-buttons {
    grid-template-columns: 1fr;
  }

  .action-buttons {
    grid-template-columns: 1fr;
  }

  .qr-grid {
    width: 150px;
    height: 150px;
  }
}
</style>
