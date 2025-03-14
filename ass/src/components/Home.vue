<script setup>
import { useRouter } from "vue-router";
import { ref, computed, onMounted } from "vue";
import axios from "axios";
import apiClient from "../axios.js";
const router = useRouter();
const danhMuc = ref([]);
const sanPhams = ref([]);
const activeTab = ref("all");
const minPrice = ref(null);
const maxPrice = ref(null);

const fetchData = async () => {
  try {
    const resDanhMuc = await axios.get("http://localhost:5242/api/Loais");
    danhMuc.value = resDanhMuc.data;

    const resSanPham = await axios.get("http://localhost:5242/api/Products", {
      params: {
        minPrice: minPrice.value,
        maxPrice: maxPrice.value,
      },
    });
    sanPhams.value = resSanPham.data;
  } catch (error) {
    console.error("Lỗi khi tải dữ liệu:", error);
  }
};

const applyFilter = async () => {
  try {
    const resSanPham = await axios.get("http://localhost:5242/api/Products", {
      params: {
        minPrice: minPrice.value || null,
        maxPrice: maxPrice.value || null,
      },
    });
    sanPhams.value = resSanPham.data;
  } catch (error) {
    console.error("Lỗi khi lọc sản phẩm:", error);
  }
};
// watch([minPrice, maxPrice], fetchData);
const filteredProducts = (loaiID) => {
  return sanPhams.value.filter((sp) => sp.loaiID === loaiID);
};

const getTenLoai = (loaiID) => {
  const loai = danhMuc.value.find((l) => l.loaiID === loaiID);
  return loai ? loai.tenLoai : "Không xác định";
};

//

//
const addToCart = async (productID) => {
  try {
    const response = await apiClient.post("/Carts/add", {
      ProductID: productID,
      SoLuong: 1,
    });

    alert(response.data.message);
  } catch (error) {
    console.error("Lỗi khi thêm vào giỏ hàng:", error);
    router.push("/login");
  }
};
onMounted(fetchData);
const slides = ref([
  {
    id: 1,
    src: "src/assets/img/carousel-1.jpg",
    title: "Organic Food Is Good For Health",
  },
  {
    id: 2,
    src: "src/assets/img/carousel-2.jpg",
    title: "Natural Food Is Always Healthy",
  },
]);
const features = ref([
  {
    image: "src/assets/img/icon-1.png",
    title: "Natural Process",
    description:
      "Tempor ut dolore lorem kasd vero ipsum sit eirmod sit. Ipsum diam justo sed vero dolor duo.",
    delay: "0.1s",
    link: "#",
  },
  {
    image: "src/assets/img/icon-2.png",
    title: "Organic Products",
    description:
      "Tempor ut dolore lorem kasd vero ipsum sit eirmod sit. Ipsum diam justo sed vero dolor duo.",
    delay: "0.3s",
    link: "#",
  },
  {
    image: "src/assets/img/icon-3.png",
    title: "Biologically Safe",
    description:
      "Tempor ut dolore lorem kasd vero ipsum sit eirmod sit. Ipsum diam justo sed vero dolor duo.",
    delay: "0.5s",
    link: "#",
  },
]);

//
</script>

<template>
  <div class="container-fluid p-0 mb-5 wow fadeIn" data-wow-delay="0.1s">
    <div id="header-carousel" class="carousel slide" data-bs-ride="carousel">
      <div class="carousel-inner">
        <div
          v-for="(slide, index) in slides"
          :key="slide.id"
          :class="['carousel-item', { active: index === 0 }]"
        >
          <img class="w-100" :src="slide.src" alt="Carousel Image" />
          <div class="carousel-caption">
            <div class="container">
              <div class="row justify-content-start">
                <div class="col-lg-7">
                  <h1 class="display-2 mb-5 animated slideInDown">
                    {{ slide.title }}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <button
        class="carousel-control-prev"
        type="button"
        data-bs-target="#header-carousel"
        data-bs-slide="prev"
      >
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="visually-hidden">Previous</span>
      </button>
      <button
        class="carousel-control-next"
        type="button"
        data-bs-target="#header-carousel"
        data-bs-slide="next"
      >
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="visually-hidden">Next</span>
      </button>
    </div>
  </div>

  <!-- Feature Start -->
  <div class="container-fluid bg-light bg-icon my-5 py-6">
    <div class="container">
      <div
        class="section-header text-center mx-auto mb-5 wow fadeInUp"
        data-wow-delay="0.1s"
        style="max-width: 500px"
      >
        <h1 class="display-5 mb-3">Our Features</h1>
        <p>
          Tempor ut dolore lorem kasd vero ipsum sit eirmod sit. Ipsum diam
          justo sed rebum vero dolor duo.
        </p>
      </div>
      <div class="row g-4">
        <div
          v-for="(feature, index) in features"
          :key="index"
          class="col-lg-4 col-md-6 wow fadeInUp"
          :data-wow-delay="feature.delay"
        >
          <div class="bg-white text-center h-100 p-4 p-xl-5">
            <img
              class="img-fluid mb-4"
              :src="feature.image"
              :alt="feature.title"
            />
            <h4 class="mb-3">{{ feature.title }}</h4>
            <p class="mb-4">{{ feature.description }}</p>
            <a
              class="btn btn-outline-primary border-2 py-2 px-4 rounded-pill"
              :href="feature.link"
              >Read More</a
            >
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Feature End -->

  <!-- product -->

  <div class="row">
    <div class="col-lg-3 border rounded p-3">
      <h3 class="text-center text-primary">Danh mục</h3>
      <ul class="nav nav-pills flex-column justify-content-start">
        <li class="nav-item">
          <a
            class="nav-link rounded-pill text-center"
            :class="{ active: activeTab === 'all' }"
            @click="activeTab = 'all'"
          >
            Tất cả
          </a>
        </li>
        <li v-for="loai in danhMuc" :key="loai.loaiID" class="nav-item">
          <a
            class="nav-link rounded-pill text-center"
            :class="{ active: activeTab === loai.loaiID }"
            @click="activeTab = loai.loaiID"
          >
            {{ loai.tenLoai }}
          </a>
        </li>
      </ul>
      <div class="mt-4 p-4">
        <h3 class="text-primary text-center mb-3">Lọc theo giá</h3>

        <div class="mb-3">
          <label for="minPrice" class="form-label fw-bold">Giá từ:</label>
          <div class="input-group">
            <span class="input-group-text">₫</span>
            <input
              type="number"
              id="minPrice"
              v-model="minPrice"
              class="form-control"
              placeholder="Nhập giá tối thiểu"
              min="0"
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="maxPrice" class="form-label fw-bold">Giá đến:</label>
          <div class="input-group">
            <span class="input-group-text">₫</span>
            <input
              type="number"
              id="maxPrice"
              v-model="maxPrice"
              class="form-control"
              placeholder="Nhập giá tối đa"
              min="0"
            />
          </div>
        </div>

        <button class="btn btn-primary w-100" @click="applyFilter">
          Áp dụng lọc
        </button>
      </div>
    </div>

    <!--  -->
    <div class="tab-content col-lg-9">
      <!-- Tab "Tất cả" Content -->
      <div v-show="activeTab === 'all'" class="row mb-4">
        <div
          v-for="sp in sanPhams"
          :key="sp.productID"
          class="col-md-6 col-lg-3 mb-4 d-flex"
        >
          <div class="border rounded p-3 w-100 d-flex flex-column">
            <div class="text-center mb-3 h-50">
              <a :href="'/Home/Privacy/' + sp.productID">
                <img
                  :src="'http://localhost:5242' + sp.hinhAnh"
                  :alt="sp.name"
                  class="img-fluid rounded"
                  style="max-height: 200px; object-fit: cover"
                />
              </a>
            </div>
            <div class="flex-grow-1">
              <h5 class="mb-2">{{ sp.name }}</h5>
              <p class="mb-1">
                <strong>Loại:</strong> {{ getTenLoai(sp.loaiID) }}
              </p>
              <p class="mb-3">
                <strong>Giá:</strong> {{ sp.gia.toLocaleString() }} VND
              </p>
            </div>
            <button
              @click="addToCart(sp.productID)"
              class="btn btn-primary w-100"
            >
              Thêm vào giỏ hàng
            </button>
          </div>
        </div>
      </div>

      <!-- Các Tab Content khác -->
      <div
        v-for="loai in danhMuc"
        :key="loai.loaiID"
        v-show="activeTab === loai.loaiID"
        class="row mb-4"
      >
        <div
          v-for="sp in filteredProducts(loai.loaiID)"
          :key="sp.productID"
          class="col-md-6 col-lg-3 mb-4 d-flex"
        >
          <div class="border rounded p-3 w-100 d-flex flex-column">
            <div class="text-center mb-3 h-50">
              <a :href="'/Home/Privacy/' + sp.productID">
                <img
                  :src="'http://localhost:5242' + sp.hinhAnh"
                  :alt="sp.name"
                  class="img-fluid rounded"
                  style="max-height: 200px; object-fit: cover"
                />
              </a>
            </div>
            <div class="flex-grow-1">
              <h5 class="mb-2">{{ sp.name }}</h5>
              <p class="mb-1"><strong>Loại:</strong> {{ loai.tenLoai }}</p>
              <p class="mb-3">
                <strong>Giá:</strong> {{ sp.gia.toLocaleString() }} VND
              </p>
            </div>
            <button
              @click="addToCart(sp.productID)"
              class="btn btn-primary w-100"
            >
              Thêm vào giỏ hàng
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
