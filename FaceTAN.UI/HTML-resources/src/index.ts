/* Vue */
import Vue from 'vue';
import router from './router';
import store from './store/store';

/* Sass Import */
//import './';

/* App Component */
import App from "./components/app.vue";

new Vue({
    el: '#app',
    router, store,

    render: h => h(App)
});
