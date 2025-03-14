import { createRouter, createWebHistory } from "vue-router";
import Home from "../components/Home.vue";
import Login from "../components/login.vue";
import TestAPI from "../components/TestAPI.vue";
import Cart from "../components/cart.vue";
import buy from "../components/buy.vue";
import viewOrder from "../components/viewOrder.vue";
import Dashboard from "../components/admin/Dashboard.vue";
import ManageOrders from "../components/admin/ManageOrders.vue";
import ManageProducts from "../components/admin/ManageProducts.vue";
import ManageUsers from "../components/admin/ManageUsers.vue";
import profile from "../components/profile.vue";
const routes = [
  { path: "/", name: "Home", component: Home },
  { path: "/login", name: "Login", component: Login },
  { path: "/test-api", name: "TestAPI", component: TestAPI },
  { path: "/buy", name: "buy", component: buy },
  { path: "/view-order", name: "viewOrder", component: viewOrder },
  { path: "/profile", name: "profile", component: profile },

  {
    path: "/cart",
    name: "Cart",
    component: Cart,
    meta: { requiresAuth: true },
  },
  {
    path: "/admin",
    name: "ManageProducts",
    component: ManageProducts,
    meta: { requiresAdmin: true, noLayout: true },
  },
  {
    path: "/users",
    name: "ManageUsers",
    component: ManageUsers,
    meta: { requiresAdmin: true, noLayout: true },
  },
  {
    path: "/orders",
    name: "ManageOrders",
    component: ManageOrders,
    meta: { requiresAdmin: true, noLayout: true },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");
  if (to.meta.requiresAdmin) {
    if (!token || role !== "admin") {
      return next({ name: "Home" });
    }
  }
  if (to.meta.requiresAuth && !token) {
    return next({ name: "Login" });
  }

  next();
});

export default router;
