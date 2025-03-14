import { createApp } from "vue";
//import "./style.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";

import "/src/assets/css/style.css";
import "/src/assets/css/bootstrap.min.css";

import "@fortawesome/fontawesome-free/css/all.css";

import App from "./App.vue";
import router from "./Router/router.js";

createApp(App).use(router).mount("#app");
