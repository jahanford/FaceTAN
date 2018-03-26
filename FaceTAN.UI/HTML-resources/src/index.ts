
/* Vue */
import Vue from "vue";
import router from './router';

/* Sass Import */
//import './';

/* App component */
import App from "./components/app.vue";

const app = new Vue({
    el: '#app',
    router,

    render: h => h(App)
});