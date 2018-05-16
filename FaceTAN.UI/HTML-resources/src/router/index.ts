import Vue from 'vue';
import VueRouter from 'vue-router';

/* Managers Page Components */ 
import DataSetManagerComponent from '../components/dataset-manager/dsm-wrapper.vue';
import TestManagerComponent from '../components/test-manager/tm-wrapper.vue';

/* API Components */
import AmazonComponent from '../components/api/amazon/index.vue';
import AzureComponent from '../components/api/azure/index.vue';
import AnimetricsComponent from '../components/api/animetrics/index.vue';
import LambdaLabsComponent from '../components/api/lambda-labs/index.vue';
import SkyBiometryComponent from '../components/api/sky-biometry/index.vue';

Vue.use(VueRouter);

declare function require(path: string, arg: any): any;

const router: VueRouter = new VueRouter({

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
            path: '/test-manager',
            name: 'test-manager',
            component: TestManagerComponent
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