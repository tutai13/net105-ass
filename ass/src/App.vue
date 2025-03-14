<script setup>
import { ref, onMounted, onUnmounted } from "vue";
import { useRoute } from "vue-router";
import { computed } from "vue";

const route = useRoute();
const isScrolled = ref(false);
const kiemTraLogin = ref(!!localStorage.getItem("token"));
console.log("Token hiện tại:", localStorage.getItem("token"));
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50;
};

onMounted(() => {
  window.addEventListener("scroll", handleScroll);
});

onUnmounted(() => {
  window.removeEventListener("scroll", handleScroll);
});
</script>

<template>
  <div v-if="$route.meta.noLayout">
    <header>
      <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <div class="container-fluid">
          <router-link class="navbar-brand" to="/admin">
            <i class="fas fa-home"></i> Admin
          </router-link>
          <button
            class="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarNav"
            aria-controls="navbarNav"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav mx-auto">
              <!-- Quản lý sản phẩm -->
              <li class="nav-item me-4">
                <router-link class="nav-link fs-5" to="/admin">
                  <i class="fas fa-box"></i> Quản lý sản phẩm
                </router-link>
              </li>
              <!-- Quản lý khách hàng -->
              <!-- <li class="nav-item me-4">
                <router-link class="nav-link fs-5" to="/users">
                  <i class="fas fa-users"></i> Quản lý khách hàng
                </router-link>
              </li> -->
              <!-- Quản lý đơn hàng -->
              <li class="nav-item me-4">
                <router-link class="nav-link fs-5" to="/orders">
                  <i class="fas fa-receipt"></i> Quản lý đơn hàng
                </router-link>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>
    <router-view />
  </div>
  <!--  -->
  <div v-else>
    <!-- Navbar -->
    <div class="container-fluid fixed-top px-0">
      <nav
        class="navbar navbar-expand-lg navbar-light py-lg-0 px-lg-5"
        :class="{ 'scrolled-nav': isScrolled }"
      >
        <router-link to="/" class="navbar-brand ms-4 ms-lg-0">
          <h1 class="fw-bold text-primary m-0">
            F<span class="text-secondary">oo</span>dy
          </h1>
        </router-link>
        <button
          type="button"
          class="navbar-toggler me-4"
          data-bs-toggle="collapse"
          data-bs-target="#navbarCollapse"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarCollapse">
          <div class="navbar-nav ms-auto p-4 p-lg-0">
            <router-link to="/" class="nav-item nav-link">Home</router-link>
            <router-link to="/about" class="nav-item nav-link"
              >About Us</router-link
            >
            <router-link to="/products" class="nav-item nav-link"
              >Products</router-link
            >
            <router-link to="/contact" class="nav-item nav-link"
              >Contact Us</router-link
            >
          </div>
          <div class="d-none d-lg-flex ms-2">
            <router-link
              :to="kiemTraLogin ? '/profile' : '/login'"
              class="btn-sm-square bg-white rounded-circle ms-3"
            >
              <small class="fa fa-user fa-lg text-body"></small>
            </router-link>
            <!-- :to="isAuthenticated ? '/profile' : '/login'"  -->

            <router-link
              to="/cart"
              class="btn-sm-square bg-white rounded-circle ms-3"
            >
              <small class="fa fa-lg fa-shopping-bag text-body"></small>
            </router-link>
          </div>
        </div>
      </nav>
    </div>

    <!-- Nội dung chính -->
    <!-- Nội dung chính -->
    <div class="main-content">
      <router-view />
    </div>

    <!-- Footer -->
    <div class="container-fluid bg-dark footer mt-5 pt-5">
      <div class="container py-5">
        <div class="row g-5">
          <div class="col-lg-3 col-md-6">
            <h1 class="fw-bold text-primary mb-4">
              F<span class="text-secondary">oo</span>dy
            </h1>
            <p>Trang web chuyên bán thực phẩm sạch, an toàn và chất lượng.</p>
            <div class="d-flex pt-2">
              <a
                class="btn btn-square btn-outline-light rounded-circle me-1"
                href="#"
              >
                <i class="fab fa-facebook-f"></i>
              </a>
              <a
                class="btn btn-square btn-outline-light rounded-circle me-1"
                href="#"
              >
                <i class="fab fa-twitter"></i>
              </a>
              <a
                class="btn btn-square btn-outline-light rounded-circle me-1"
                href="#"
              >
                <i class="fab fa-instagram"></i>
              </a>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <h4 class="text-light mb-4">Địa chỉ</h4>
            <p>
              <i class="fa fa-map-marker-alt me-3"></i>123 Đường ABC, TP.HCM
            </p>
            <p><i class="fa fa-phone-alt me-3"></i>+84 123 456 789</p>
            <p><i class="fa fa-envelope me-3"></i>info@foody.com</p>
          </div>
          <div class="col-lg-3 col-md-6">
            <h4 class="text-light mb-4">Liên kết nhanh</h4>
            <a class="btn btn-link" href="#">Về chúng tôi</a>
            <a class="btn btn-link" href="#">Liên hệ</a>
            <a class="btn btn-link" href="#">Dịch vụ</a>
            <a class="btn btn-link" href="#">Chính sách</a>
          </div>
          <div class="col-lg-3 col-md-6">
            <h4 class="text-light mb-4">Đăng ký nhận tin</h4>
            <p>Nhận thông tin khuyến mãi và sản phẩm mới nhất.</p>
            <div class="position-relative" style="max-width: 400px">
              <input
                class="form-control bg-transparent w-100 py-3 ps-4 pe-5"
                type="text"
                placeholder="Nhập email"
              />
              <button
                type="button"
                class="btn btn-primary py-2 position-absolute top-0 end-0 mt-2 me-2"
              >
                Đăng ký
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
/* Navbar mặc định trong suốt */
.navbar {
  background: transparent;
  transition: background 0.3s ease-in-out, box-shadow 0.3s ease-in-out;
}
.main-content {
  padding-top: 70px;
}
/* Khi cuộn xuống */
.scrolled-nav {
  background: rgba(255, 255, 255, 0.95);
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}
</style>
