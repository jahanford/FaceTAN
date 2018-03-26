import Vue from 'vue';
import VueRouter from 'vue-router';

import DataSetManagerComponent from '../components/dataset-manager.vue';

/* API Components */
import AmazonComponent from '../components/amazon/index.vue';
import AzureComponent from '../components/azure/index.vue';
import AnimetricsComponent from '../components/animetrics/index.vue';
import LambdaLabsComponent from '../components/lambda-labs/index.vue';
import SkyBiometryComponent from '../components/sky-biometry/index.vue';

Vue.use(VueRouter);

declare function require(path: string, arg: any): any;

const router = new VueRouter({

    routes: [
        {
            /* Generic Redirect */
            path: '*',
            redirect: '/dataset-manager'     
        },
        {
            path: '/dataset-manager',
            name: 'dataset-manager',
            component: DataSetManagerComponent     
        },
        {
            path: '/amazon',
            name: 'amazon',
            component: AmazonComponent
        },
        {
            path: '/azure',
            name: 'azure',
            component: AzureComponent
        },
        {
            path: '/animetrics',
            name: 'animetrics',
            component: AnimetricsComponent
        },
        {
            path: '/lambda-labs',
            name: 'lambda-labs',
            component: LambdaLabsComponent
        },
        {
            path: '/sky-biometry',
            name: 'sky-biometry',
            component: SkyBiometryComponent
        }
    ]
});

export default router;