<template>
  <div class="cart">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="mb-0">Giỏ Hàng</h2>
      <button @click="redirectToOrder" class="btn btn-success">
        Đơn Hàng Của Bạn
      </button>
    </div>

    <table class="table">
      <thead>
        <tr>
          <th>Sản Phẩm</th>
          <th>Đơn Giá</th>
          <th>Số Lượng</th>
          <th>Số Tiền</th>
          <th>Thao tác</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in carts" :key="item.cartID">
          <td>
            <div class="product-image d-flex align-items-center mt-2">
              <img
                :src="'http://localhost:5242' + item.hinhAnh"
                :alt="item.tenSanPham"
                class="img-fluid rounded-circle"
                style="width: 90px; height: 90px"
              />
              <h5 class="ml-2">{{ item.tenSanPham }}</h5>
            </div>
          </td>
          <td>{{ formatPrice(item.gia) }}</td>
          <td>
            <input
              type="number"
              class="quantity-input"
              v-model.number="item.soLuong"
              @change="updateQuantity(item)"
              min="1"
              style="width: 60px"
            />
          </td>
          <td class="total-price">
            {{ formatPrice(item.gia * item.soLuong) }}
          </td>
          <td>
            <button
              @click="deleteCartItem(item.cartID)"
              class="btn btn-md rounded-circle bg-light border"
            >
              <i class="fa fa-times text-danger"></i>
            </button>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="3" class="text-right"><strong>Tổng Tiền:</strong></td>
          <td colspan="2" class="text-left">
            <strong>{{ formatPrice(totalAmount) }}</strong>
          </td>
        </tr>
      </tfoot>
    </table>

    <div class="d-flex justify-content-end">
      <button @click="buyCart" class="btn btn-success">Mua Hàng</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import axios from "axios";
import apiClient from "../axios.js";
import { useRouter } from "vue-router";

const carts = ref([]);
const router = useRouter();

const totalAmount = computed(() =>
  carts.value.reduce((total, item) => total + item.gia * item.soLuong, 0)
);

const formatPrice = (value) => {
  return Number(value).toLocaleString();
};

const checkLoginStatus = () => {
  const loggedInUser = localStorage.getItem("token");
  if (!loggedInUser) {
    alert("Bạn cần đăng nhập để truy cập trang này!");
    router.push("/login");
  }
};

const fetchCarts = async () => {
  try {
    const response = await apiClient.get("/Carts");
    carts.value = response.data;
  } catch (error) {
    console.error("Lỗi khi lấy giỏ hàng:", error);
    router.push("/login");
  }
};

const updateQuantity = async (item) => {
  if (item.soLuong < 1) {
    alert("Số lượng phải lớn hơn 0.");
    item.soLuong = 1;
    return;
  }
  try {
    const url = `/Carts/update?CartID=${encodeURIComponent(
      item.cartID
    )}&soLuong=${encodeURIComponent(item.soLuong)}`;
    const response = await apiClient.post(url);
    if (response.data.success) {
      alert(response.data.message);
      await fetchCarts();
    } else {
      alert(response.data.message);
    }
  } catch (error) {
    alert("Đã xảy ra lỗi, vui lòng thử lại.");
  }
};

const deleteCartItem = async (cartID) => {
  if (!cartID) {
    console.error("cartID bị undefined!");
    return;
  }
  try {
    const response = await apiClient.delete(`/Carts/remove/${cartID}`);
    if (response.data.success) {
      alert(response.data.message);
      await fetchCarts();
    } else {
      alert(response.data.message);
    }
  } catch (error) {
    alert("Đã xảy ra lỗi khi xóa sản phẩm khỏi giỏ hàng.");
    console.error(error);
  }
};

const buyCart = () => {
  router.push("/buy");
};

const redirectToOrder = () => {
  router.push("/view-order");
};

onMounted(() => {
  fetchCarts();
  checkLoginStatus();
});
</script>

<style scoped>
.product-image img {
  width: 100px;
  height: 100px;
  object-fit: cover;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
</style>
