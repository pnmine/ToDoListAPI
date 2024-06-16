import Axios from "axios";

const axios = Axios.create({
  baseURL: "http://localhost:5245/api", // URL ของ API ของคุณ
});

export { axios };
export default axios;
