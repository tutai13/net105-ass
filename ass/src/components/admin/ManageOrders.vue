<template>
  <div>
    <h2 class="text-center">Danh Sách Đơn Hàng</h2>
    <!-- Bộ lọc ngày -->
    <div class="container d-flex gap-2 mb-3">
      <input type="date" v-model="startDate" class="form-control" />
      <input type="date" v-model="endDate" class="form-control" />
      <button class="btn btn-primary" @click="fetchOrders">Lọc</button>
    </div>

    <!-- 
     -->
    <div v-if="loading" class="text-center">
      <i class="fas fa-spinner fa-spin"></i> Đang tải...
    </div>
    <div v-else-if="errorMessage" class="alert alert-danger">
      {{ errorMessage }}
    </div>

    <table v-else class="table table-bordered table-hover">
      <thead class="table-primary">
        <tr>
          <th>#</th>
          <th>Người Đặt</th>
          <th>Ngày Tạo</th>
          <th>Tổng Tiền</th>
          <th>Trạng Thái</th>
          <th>Số Điện Thoại</th>
          <th>Ghi Chú</th>
          <th>Thao Tác</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(order, index) in orders" :key="order.hoaDonID">
          <td>{{ index + 1 }}</td>
          <td>{{ order.fullName }}</td>
          <!-- Hiển thị tên khách hàng -->
          <td>{{ formatDate(order.ngayTao) }}</td>
          <td>{{ formatCurrency(order.tongTien) }}</td>
          <td>
            <span
              v-if="order.trangThai === 'Đang Giao Hàng'"
              class="text-success"
            >
              <i class="fas fa-truck"></i> Đang Giao Hàng
            </span>
            <span v-else class="text-info">
              <i class="fas fa-spinner fa-spin"></i> {{ order.trangThai }}
            </span>
          </td>
          <td>{{ order.soDienThoai }}</td>
          <td>{{ order.ghiChu || "Không có ghi chú" }}</td>
          <td>
            <div class="d-flex gap-2">
              <button class="btn btn-info btn-sm" @click="toggleDetails(order)">
                <i class="fas fa-eye"></i> Xem Chi Tiết
              </button>
              <button
                v-if="order.trangThai !== 'Đang Giao Hàng'"
                class="btn btn-success btn-sm"
                @click="confirmDelivery(order.hoaDonID)"
              >
                <i class="fas fa-truck"></i> Xác Nhận Giao Hàng
              </button>
              <button
                v-if="order.trangThai !== 'Đang Giao Hàng'"
                class="btn btn-danger btn-sm"
                @click="cancelOrder(order.hoaDonID)"
              >
                <i class="fas fa-times"></i> Hủy
              </button>
            </div>
          </td>
        </tr>
        <tr v-if="orders.length === 0">
          <td colspan="8" class="text-center">Bạn chưa có đơn hàng nào.</td>
        </tr>
      </tbody>
    </table>

    <!-- Chi tiết đơn hàng -->
    <div v-if="selectedOrder" class="card mt-3">
      <div
        class="card-header bg-dark text-white d-flex justify-content-between align-items-center"
      >
        <h5>Chi Tiết Đơn Hàng #{{ selectedOrder.hoaDonID }}</h5>
        <button
          class="btn-close btn-close-white"
          @click="selectedOrder = null"
        ></button>
      </div>
      <div class="card-body">
        <table class="table table-sm">
          <thead>
            <tr>
              <th>#</th>
              <th>Hình Ảnh</th>
              <th>Tên Sản Phẩm</th>
              <th>Giá</th>
              <th>Số Lượng</th>
              <th>Thành Tiền</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, subIndex) in selectedOrder.chiTiet"
              :key="item.sanPhamID"
            >
              <td>{{ subIndex + 1 }}</td>
              <td>
                <img
                  :src="'http://localhost:5242' + item.hinhAnh"
                  :alt="item.tenSanPham"
                  class="img-thumbnail"
                  style="width: 80px; height: 80px"
                />
              </td>
              <td>{{ item.tenSanPham }}</td>
              <td>{{ formatCurrency(item.gia) }}</td>
              <td>{{ item.soLuong }}</td>
              <td>{{ formatCurrency(item.soLuong * item.gia) }}</td>
            </tr>
            <tr v-if="selectedOrder.chiTiet.length === 0">
              <td colspan="6" class="text-center">
                Không có sản phẩm nào trong hóa đơn này.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import apiClient from "../../axios";

// Khai báo reactive variables
const orders = ref([]);
const loading = ref(true);
const errorMessage = ref("");
const selectedOrder = ref(null);
const startDate = ref("");
const endDate = ref("");

// const fetchOrders = async () => {
//   try {
//     const response = await apiClient.get(`/HoaDon/adminViewOrder`);
//     orders.value = response.data;
//   } catch (error) {
//     errorMessage.value = "Không thể tải danh sách đơn hàng.";
//   } finally {
//     loading.value = false;
//   }
// };
const fetchOrders = async () => {
  loading.value = true;
  console.log("startDate:", startDate.value, "endDate:", endDate.value); // Kiểm tra giá trị

  try {
    const response = await apiClient.get("/HoaDon/adminViewOrderFiltered", {
      params: {
        startDate: startDate.value ? startDate.value : null, // Đảm bảo giá trị hợp lệ
        endDate: endDate.value ? endDate.value : null,
      },
    });

    console.log("API response:", response.data);
    orders.value = response.data;
  } catch (error) {
    console.error("Lỗi API:", error);
    errorMessage.value = "Không thể tải danh sách đơn hàng.";
  } finally {
    loading.value = false;
  }
};

// Hàm xem chi tiết đơn hàng
const toggleDetails = (order) => {
  selectedOrder.value =
    selectedOrder.value?.hoaDonID === order.hoaDonID ? null : order;
};

// Hàm hủy đơn hàng
const cancelOrder = async (orderId) => {
  if (!confirm("Bạn có chắc chắn muốn hủy đơn hàng này?")) return;

  try {
    await axios.post(`http://localhost:5242/api/HoaDon/cancelOrder/${orderId}`);
    orders.value = orders.value.filter((o) => o.hoaDonID !== orderId);
    alert("Đơn hàng đã bị hủy.");
  } catch (error) {
    alert("Lỗi khi hủy đơn hàng.");
  }
};

//
const confirmDelivery = async (orderId) => {
  if (!confirm("Bạn có chắc chắn muốn xác nhận đơn hàng này?")) return;

  try {
    await apiClient.post(`/HoaDon/confirmDelivery/${orderId}`);
    const order = orders.value.find((o) => o.hoaDonID === orderId);
    if (order) {
      order.trangThai = "Đang Giao Hàng";
    }
    alert("Đơn hàng đã được xác nhận đang giao.");
  } catch (error) {
    alert("Lỗi khi cập nhật đơn hàng.");
  }
};

// Format ngày tháng
const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString("vi-VN");
};

// Format tiền VND
const formatCurrency = (amount) => {
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(amount);
};

// Gọi API khi component mounted
onMounted(fetchOrders);
</script>

<style scoped>
.btn-close {
  float: right;
}
</style>
