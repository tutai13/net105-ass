<template>
  <div class="container">
    <!-- Bố cục chia đôi -->
    <div class="content">
      <!-- Cột trái (30%) -->
      <div class="form-container card shadow-sm p-4">
        <h2 class="mb-4">
          {{ isEditing ? "Cập nhật sản phẩm" : "Thêm sản phẩm" }}
        </h2>

        <form @submit.prevent="saveProduct">
          <div class="mb-3">
            <label class="form-label">Tên sản phẩm:</label>
            <input v-model="product.name" class="form-control" required />
          </div>

          <div class="mb-3">
            <label class="form-label">Chi tiết:</label>
            <textarea
              v-model="product.chiTiet"
              class="form-control"
              rows="3"
              required
            ></textarea>
          </div>

          <div class="mb-3">
            <label class="form-label">Giá:</label>
            <input
              type="number"
              v-model="product.gia"
              class="form-control"
              min="0"
              required
            />
          </div>

          <div class="mb-3">
            <label class="form-label">Hình ảnh (URL):</label>
            <input v-model="product.hinhAnh" class="form-control" required />
          </div>

          <div class="mb-3">
            <label class="form-label">Loại sản phẩm:</label>
            <!-- <select v-model="product.loaiID" class="form-select" required>
              <option
                v-for="loai in loailist"
                :key="loai.LoaiID"
                :value="loai.LoaiID"
              >
                {{ loai.tenLoai }}
              </option>
            </select> -->
            <input
              type="number"
              v-model="product.loaiID"
              class="form-control"
              required
            />
          </div>

          <div class="d-flex gap-2">
            <button type="submit" class="btn btn-primary">
              {{ isEditing ? "Cập nhật" : "Thêm" }}
            </button>
            <button
              v-if="isEditing"
              @click="cancelEdit"
              type="button"
              class="btn btn-secondary"
            >
              Hủy
            </button>
          </div>
        </form>
      </div>

      <!-- Cột phải (70%) -->
      <div class="table-container">
        <h2>Danh sách sản phẩm</h2>
        <div class="search-bar mb-3">
          <input
            v-model="searchQuery"
            @input="fetchProducts"
            type="text"
            class="form-control"
            placeholder="Tìm kiếm sản phẩm..."
          />
        </div>

        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Tên</th>
              <th>Chi tiết</th>
              <th>Giá</th>
              <th>Hình ảnh</th>
              <th>Loại ID</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="p in products" :key="p.productID">
              <td>{{ p.productID }}</td>
              <td>{{ p.name }}</td>
              <td>{{ p.chiTiet }}</td>
              <td>{{ p.gia }} VND</td>
              <td>
                <img
                  :src="'http://localhost:5242' + p.hinhAnh"
                  alt="Hình ảnh"
                  width="60"
                />
              </td>
              <td>{{ p.tenLoai }}</td>
              <td>
                <button @click="editProduct(p)">Sửa</button>
                <button @click="deleteProduct(p.productID)">Xóa</button>
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

const products = ref([]);
const loailist = ref([]);
const product = ref({
  productID: 0,
  name: "",
  chiTiet: "",
  gia: 0,
  hinhAnh: "",
  loaiID: 0,
});
const isEditing = ref(false);
const searchQuery = ref("");
// Lấy danh sách sản phẩm
const fetchProducts = async () => {
  try {
    const searchParam = searchQuery.value ? `?search=${searchQuery.value}` : "";
    const [productRes, loaiRes] = await Promise.all([
      axios.get(`http://localhost:5242/api/Products${searchParam}`),
      axios.get("http://localhost:5242/api/loais"),
    ]);

    products.value = productRes.data;
    loailist.value = loaiRes.data;
  } catch (error) {
    console.error("Lỗi khi lấy sản phẩm:", error);
  }
};

// Thêm / Cập nhật sản phẩm
const saveProduct = async () => {
  if (isEditing.value) {
    try {
      await apiClient.put(
        `/Products/${product.value.productID}`,
        product.value
      );
      fetchProducts();
      cancelEdit();
    } catch (error) {
      console.error("Lỗi khi cập nhật sản phẩm:", error);
    }
  } else {
    try {
      await apiClient.post("/Products", product.value);
      fetchProducts();
      resetForm();
    } catch (error) {
      console.error("Lỗi khi thêm sản phẩm:", error);
    }
  }
};

// Chỉnh sửa sản phẩm
const editProduct = (p) => {
  // const loai = loailist.value.find((l) => Number(l.loaiID) == Number(p.loaiID));
  // const tenloai = loai.tenLoai;

  // product.value = {
  //   productID: p.productID,
  //   name: p.name,
  //   chiTiet: p.chiTiet,
  //   gia: p.gia,
  //   hinhAnh: p.hinhAnh,
  //   loaiID: p.loaiID,
  //   // tenLoai: loai ? loai.tenLoai : "Không xác định",
  // };
  product.value = { ...p };
  isEditing.value = true;
};

// Xóa sản phẩm
const deleteProduct = async (id) => {
  if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
    try {
      await apiClient.delete(`/Products/${id}`);
      fetchProducts();
    } catch (error) {
      console.error("Lỗi khi xóa sản phẩm:", error);
    }
  }
};

// Hủy chỉnh sửa
const cancelEdit = () => {
  resetForm();
  isEditing.value = false;
};

// Reset form về trạng thái ban đầu
const resetForm = () => {
  product.value = {
    productID: null,
    name: "",
    chiTiet: "",
    gia: 0,
    hinhAnh: "",
    loaiID: 0,
  };
};

// Gọi API khi component được mount
onMounted(fetchProducts);
</script>

<style scoped>
.container {
  max-width: 1200px;
  margin: auto;
  padding: 20px;
}
.content {
  display: flex;
  gap: 20px;
}

/* Cột trái - Form (30%) */
.form-container {
  flex: 3;
  background: #f8f9fa;
  padding: 20px;
  border-radius: 8px;
}
form {
  display: flex;
  flex-direction: column;
  gap: 10px;
}
button {
  padding: 5px 10px;
  margin-top: 10px;
}

/* Cột phải - Danh sách sản phẩm (70%) */
.table-container {
  flex: 7;
}
table {
  width: 100%;
  border-collapse: collapse;
}
th,
td {
  border: 1px solid #ddd;
  padding: 8px;
}
th {
  background: #f4f4f4;
}
</style>
