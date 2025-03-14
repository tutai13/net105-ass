// src/axios.js
import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:7102/api", // Lưu ý: Sử dụng https nếu server của bạn chạy trên https
  timeout: 10000,
});

// Interceptor thêm header Authorization vào request
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default apiClient;
