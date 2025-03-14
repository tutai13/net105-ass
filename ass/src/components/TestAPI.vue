<template>
  <div class="container">
    <h1>Kiểm tra kết nối API</h1>
    <button @click="testAPI">Gửi yêu cầu</button>
    <p v-if="message">{{ message }}</p>
    <p v-if="error" class="error">{{ error }}</p>
    <button @click="logout">logout</button>
  </div>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";

const message = ref("");
const error = ref("");

// Hàm kiểm tra kết nối API
const testAPI = async () => {
  try {
    const response = await axios.get("http://localhost:5242/api/auth/test");
    message.value = response.data.message;
    error.value = "";
  } catch (err) {
    console.error("Lỗi:", err);
    error.value = "Không thể kết nối với API!";
  }
};

const logout = async () => {
  localStorage.removeItem("token");
};
</script>

<style scoped>
.container {
  text-align: center;
  margin-top: 50px;
}

button {
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
}

.error {
  color: red;
  font-weight: bold;
}
</style>
