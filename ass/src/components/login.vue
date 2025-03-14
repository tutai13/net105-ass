<template>
  <div class="container">
    <div class="wrapper">
      <div class="card-switch">
        <label class="switch">
          <input type="checkbox" class="toggle" v-model="isSignUp" />
          <span class="slider"></span>
          <span class="card-side"></span>
          <div class="flip-card__inner" :class="{ flipped: isSignUp }">
            <!-- Đăng nhập -->

            <div class="flip-card__front">
              <div class="title">Log in</div>
              <form class="flip-card__form" @submit.prevent="handleLogin">
                <input
                  v-model="loginData.username"
                  class="flip-card__input"
                  placeholder="Username"
                  type="text"
                  required
                />
                <input
                  v-model="loginData.password"
                  class="flip-card__input"
                  placeholder="Password"
                  type="password"
                  required
                />
                <button class="flip-card__btn" type="submit">Let’s go!</button>
              </form>
              <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
            </div>

            <!-- Đăng ký -->
            <div class="flip-card__back">
              <div class="title">Sign up</div>
              <form class="flip-card__form" @submit.prevent="handleSignUp">
                <input
                  v-model="signUpData.name"
                  class="flip-card__input"
                  placeholder="Name"
                  type="text"
                  required
                />
                <input
                  v-model="signUpData.username"
                  class="flip-card__input"
                  placeholder="Email"
                  type="email"
                  required
                />
                <input
                  v-model="signUpData.phoneNumber"
                  class="flip-card__input"
                  placeholder="Phone Number"
                  type="tel"
                  required
                />
                <input
                  v-model="signUpData.password"
                  class="flip-card__input"
                  placeholder="Password"
                  type="password"
                  required
                />
                <input
                  v-model="signUpData.confirmPassword"
                  class="flip-card__input"
                  placeholder="Confirm Password"
                  type="password"
                  required
                />
                <button class="flip-card__btn" type="submit">Confirm!</button>
              </form>
              <p v-if="signUpErrorMessage" class="error">
                {{ signUpErrorMessage }}
              </p>
            </div>
          </div>
        </label>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

const router = useRouter();
const isSignUp = ref(false);

const loginData = ref({
  username: "",
  password: "",
});

const signUpData = ref({
  name: "",
  email: "",
  phoneNumber: "",
  password: "",
  confirmPassword: "",
});

const errorMessage = ref("");
const signUpErrorMessage = ref("");

// Đăng nhập
const handleLogin = async () => {
  try {
    const response = await axios.post("http://localhost:5242/api/auth/login", {
      username: loginData.value.username,
      password: loginData.value.password,
    });

    if (response.data.token) {
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("role", response.data.role);
      alert("Đăng nhập thành công!");
      router.push("/");
    } else {
      errorMessage.value = "Username hoặc mật khẩu không hợp lệ.";
    }
  } catch (error) {
    console.error(error);
    errorMessage.value = "Không thể kết nối với máy chủ.";
  }
};

// Đăng ký
const handleSignUp = async () => {
  try {
    const response = await axios.post(
      "http://localhost:5242/api/auth/register",
      {
        fullName: signUpData.value.name,
        email: signUpData.value.username,
        phoneNumber: signUpData.value.phoneNumber,
        password: signUpData.value.password,
        confirmPassword: signUpData.value.confirmPassword,
      }
    );

    if (response.status === 200) {
      alert("Đăng ký thành công! Hãy đăng nhập.");
      isSignUp.value = false;
    } else {
      signUpErrorMessage.value = "Đăng ký không thành công.";
    }
  } catch (error) {
    console.error(error);
    signUpErrorMessage.value = "Lỗi kết nối đến máy chủ.";
  }
};
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap");

* {
  font-family: "Poppins", sans-serif;
}
.container {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
}
.wrapper {
  --input-focus: #f02df0;
  --font-color: #323232;
  --font-color-sub: #666;
  --bg-color: #fff;
  --bg-color-alt: #666;
  --main-color: #323232;
}
/* switch card */
.switch {
  transform: translateY(-200px);
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 30px;
  width: 50px;
  height: 20px;
}
/*Andev Web*/
.card-side::before {
  position: absolute;
  content: "Log in";
  left: -70px;
  top: 0;
  width: 100px;
  text-decoration: underline;
  color: var(--font-color);
  font-weight: 600;
}
.card-side::after {
  position: absolute;
  content: "Sign up";
  left: 70px;
  top: 0;
  width: 100px;
  text-decoration: none;
  color: var(--font-color);
  font-weight: 600;
}
.toggle {
  opacity: 0;
  width: 0;
  height: 0;
}
.slider {
  box-sizing: border-box;
  border-radius: 5px;
  border: 2px solid var(--main-color);
  box-shadow: 4px 4px var(--main-color);
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: var(--bg-colorcolor);
  transition: 0.3s;
}
.slider:before {
  box-sizing: border-box;
  position: absolute;
  content: "";
  height: 20px;
  width: 20px;
  border: 2px solid var(--main-color);
  border-radius: 5px;
  left: -2px;
  bottom: 2px;
  background-color: var(--bg-color);
  box-shadow: 0 3px 0 var(--main-color);
  transition: 0.3s;
}
.toggle:checked + .slider {
  background-color: var(--input-focus);
}
.toggle:checked + .slider:before {
  transform: translateX(30px);
}
.toggle:checked ~ .card-side:before {
  text-decoration: none;
}
.toggle:checked ~ .card-side:after {
  text-decoration: underline;
}
/* card */
.flip-card__inner {
  width: 300px;
  height: 350px;
  position: relative;
  background-color: transparent;
  perspective: 1000px;
  text-align: center;
  transition: transform 0.8s;
  transform-style: preserve-3d;
}
.toggle:checked ~ .flip-card__inner {
  transform: rotateY(180deg);
}
.toggle:checked ~ .flip-card__front {
  box-shadow: none;
}
.flip-card__front,
.flip-card__back {
  padding: 20px;
  position: absolute;
  display: flex;
  flex-direction: column;
  justify-content: center;
  -webkit-backface-visibility: hidden;
  backface-visibility: hidden;
  background: lightgrey;
  gap: 20px;
  border-radius: 5px;
  border: 2px solid var(--main-color);
  box-shadow: 4px 4px var(--main-color);
}
.flip-card__back {
  width: 100%;
  transform: rotateY(180deg);
}
.flip-card__form {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
}
.title {
  margin: 20px 0 20px 0;
  font-size: 25px;
  font-weight: 900;
  text-align: center;
  color: var(--main-color);
}
.flip-card__input {
  width: 250px;
  height: 40px;
  border-radius: 5px;
  border: 2px solid var(--main-color);
  background-color: var(--bg-color);
  box-shadow: 4px 4px var(--main-color);
  font-size: 15px;
  font-weight: 600;
  color: var(--font-color);
  padding: 5px 10px;
  outline: none;
}
.flip-card__input::placeholder {
  color: var(--font-color-sub);
  opacity: 0.8;
}
.flip-card__input:focus {
  border: 2px solid var(--input-focus);
}
.flip-card__btn:active,
.button-confirm:active {
  box-shadow: 0px 0px var(--main-color);
  transform: translate(3px, 3px);
}
.flip-card__btn {
  margin: 20px 0 20px 0;
  width: 120px;
  height: 40px;
  border-radius: 5px;
  border: 2px solid var(--main-color);
  background-color: var(--bg-color);
  box-shadow: 4px 4px var(--main-color);
  font-size: 17px;
  font-weight: 600;
  color: var(--font-color);
  cursor: pointer;
}
</style>
