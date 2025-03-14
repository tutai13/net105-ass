<template>
  <div class="checkout-container">
    <h2 class="mb-4 text-center">Thanh Toán</h2>

    <div v-if="order.length > 0">
      <div class="customer-info card p-3 mb-4">
        <h5>Thông Tin Khách Hàng</h5>
        <p><strong>Họ Tên:</strong> {{ order[0].fullName }}</p>
        <p><strong>Email:</strong> {{ order[0].email }}</p>
        <p><strong>Số Điện Thoại:</strong> {{ order[0].phoneNumber }}</p>
      </div>

      <div class="form-group">
        <label>Phương Thức Thanh Toán</label>
        <select v-model="paymentMethod" class="form-select" required>
          <option value="" disabled>Chọn Phương Thức Thanh Toán</option>
          <option value="1">Thanh Toán Khi Nhận Hàng</option>
          <option value="2">Thẻ tín dụng/ Thẻ ghi nợ</option>
          <option value="3">VietQR</option>
        </select>
      </div>

      <div class="form-group">
        <label>Ghi Chú</label>
        <textarea v-model="note" class="form-control"></textarea>
      </div>

      <h4 class="mt-4">Danh Sách Sản Phẩm</h4>
      <table class="table">
        <thead>
          <tr>
            <th>Sản Phẩm</th>
            <th>Hình Ảnh</th>
            <th>Giá</th>
            <th>Số Lượng</th>
            <th>Tổng Tiền</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in order" :key="item.productName">
            <td>{{ item.productName }}</td>
            <td>
              <img
                :src="'http://localhost:5242' + item.hinhAnh"
                :alt="item.ProductName"
                class="product-image"
              />
            </td>
            <td>{{ formatPrice(item.gia) }}</td>
            <td>{{ item.soLuong }}</td>
            <td>{{ formatPrice(item.totalPrice) }}</td>
          </tr>
        </tbody>
      </table>

      <div class="text-right mt-3">
        <h4>
          <strong>Tổng Tiền: {{ formatPrice(totalAmount) }}</strong>
        </h4>
      </div>

      <button @click="submitOrder" class="btn btn-primary mt-3">
        Thanh Toán
      </button>
    </div>

    <div v-else class="alert alert-warning">
      Không có sản phẩm nào trong giỏ hàng.
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import apiClient from "../axios.js";
import { useRouter } from "vue-router";
const router = useRouter();
const loading = ref(false);
const order = ref([]);
const paymentMethod = ref("");
const note = ref("");

const totalAmount = computed(() =>
  order.value.reduce((total, item) => total + item.totalPrice, 0)
);

const formatPrice = (value) => {
  return Number(value).toLocaleString() + " VNĐ";
};

const fetchOrder = async () => {
  try {
    const response = await apiClient.get("/carts/order");
    order.value = response.data;
  } catch (error) {
    console.error("Lỗi khi lấy đơn hàng:", error);
  }
};

const submitOrder = async () => {
  if (!paymentMethod.value) {
    alert("Vui lòng chọn phương thức thanh toán!");
    return;
  }

  loading.value = true;
  const orderData = {
    fullName: order.value[0].fullName,
    email: order.value[0].email,
    phoneNumber: order.value[0].phoneNumber,
    ghiChu: note.value,
  };

  try {
    const response = await apiClient.post("/carts/pay", orderData);
    alert("Thanh toán thành công! Mã đơn hàng: " + response.data.invoiceId);
    router.push("/"); // Chuyển hướng đến trang lịch sử đơn hàng
  } catch (error) {
    console.error("Lỗi khi thanh toán:", error);
    alert("Có lỗi xảy ra khi thanh toán.");
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  fetchOrder();
});
</script>

<style scoped>
.checkout-container {
  max-width: 800px;
  margin: 0 auto;
}

.customer-info {
  background-color: #f8f9fa;
  border-radius: 8px;
}

.product-image {
  width: 80px;
  height: 80px;
  object-fit: cover;
  border-radius: 8px;
}

.form-group {
  margin-bottom: 15px;
}
</style>
