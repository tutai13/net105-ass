<template>
  <div class="container mt-5">
    <h2 class="text-center">Thông tin người dùng</h2>

    <div v-if="loading" class="text-center">
      <p>Đang tải...</p>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else class="card p-4 shadow">
      <div v-if="!isEditing">
        <p><strong>Họ tên:</strong> {{ user.fullName }}</p>
        <p><strong>Email:</strong> {{ user.email }}</p>
        <p><strong>Số điện thoại:</strong> {{ user.phoneNumber }}</p>

        <div class="text-center mt-3">
          <button @click="startEdit" class="btn btn-primary me-2">
            Cập nhật
          </button>
          <button @click="logout" class="btn btn-danger">Logout</button>
        </div>
      </div>

      <div v-else>
        <div class="mb-3">
          <label class="form-label">Họ tên</label>
          <input v-model="editData.fullName" type="text" class="form-control" />
        </div>
        <div class="mb-3">
          <label class="form-label">Số điện thoại</label>
          <input
            v-model="editData.phoneNumber"
            type="text"
            class="form-control"
          />
        </div>

        <div class="text-center mt-3">
          <button @click="updateUser" class="btn btn-success me-2">Lưu</button>
          <button @click="cancelEdit" class="btn btn-secondary">Hủy</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import apiClient from "../axios";

const router = useRouter();
const user = ref(null);
const editData = ref({ fullName: "", phoneNumber: "" });
const error = ref(null);
const loading = ref(true);
const isEditing = ref(false);

onMounted(async () => {
  await fetchUser();
});

const fetchUser = async () => {
  const token = localStorage.getItem("token");
  if (!token) {
    router.push("/login");
    return;
  }

  try {
    const response = await apiClient.get("/auth/user");
    user.value = response.data;
  } catch (err) {
    error.value =
      err.response?.data?.message || "Đã xảy ra lỗi khi lấy thông tin";
    router.push("/login");
  } finally {
    loading.value = false;
  }
};

const startEdit = () => {
  editData.value = { ...user.value }; // Copy dữ liệu từ user vào form
  isEditing.value = true;
};

const cancelEdit = () => {
  isEditing.value = false;
};

const updateUser = async () => {
  const token = localStorage.getItem("token");

  try {
    await apiClient.put("/auth/update", {
      fullName: editData.value.fullName,
      phoneNumber: editData.value.phoneNumber,
    });

    await fetchUser(); // Refresh lại thông tin
    isEditing.value = false;
  } catch (err) {
    error.value = err.response?.data?.message || "Lỗi khi cập nhật thông tin";
  }
};

const logout = () => {
  localStorage.removeItem("token");
  router.push("/login");
  window.location.reload();
};
</script>

<style scoped>
.container {
  max-width: 500px;
  margin: auto;
}
</style>
