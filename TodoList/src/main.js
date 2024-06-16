import "./assets/main.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import BoxShadow from "./components/layout/BoxShadow.vue";
import { OhVueIcon, addIcons } from "oh-vue-icons";
import { CoListNumberedRtl } from "oh-vue-icons/icons";
import { FaFlag, RiZhihuFill } from "oh-vue-icons/icons";
import { MdCloseRound } from "oh-vue-icons/icons";
import { RiFileList3Line } from "oh-vue-icons/icons";
import axios from "./api";

addIcons(FaFlag, RiZhihuFill,CoListNumberedRtl,MdCloseRound,RiFileList3Line);

const app = createApp(App);

app.use(router);

app.provide('axios',axios)

app.component("v-icon", OhVueIcon);
app.component("box-shadow", BoxShadow);

app.mount("#app");
