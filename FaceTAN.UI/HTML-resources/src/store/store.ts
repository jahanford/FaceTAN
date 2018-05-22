import Vue from 'vue';
import Vuex from 'vuex';
import { GetterTree, MutationTree, ActionTree,  } from 'vuex';
import * as T from '../models/models';
import * as BindingObject from '../models/BindingObject';
import GlobalHelpers from '../GlobalHelpers';

let Global = new GlobalHelpers();

Vue.use(Vuex)

interface State {  
    
    dsmDataset: T.ImageList;
    dsmSubsets: T.ImageList[];

    tmTests: T.Test[];

    dsmView: T.DsmView;
}

const getters: GetterTree<State, any> = {
    
    getDataSet: (state, getters) => {
        return state.dsmDataset;
    },
    getSubsets: (state, getters) => {
        return state.dsmSubsets;
    },
    getSubset: (state, getters) => {
        return (searchGuid: string) => state.dsmSubsets.filter(subset => {
            return subset.guid == searchGuid;
        });
    },
    getTests: (state, getters) => {
        return state.tmTests;
    },
    getView: (state, getters) => {
        return state.dsmView;
    }

}

const mutations: MutationTree<State> = {
    
    addDataSetImage: (state, payload: T.ImageElement) => {
        state.dsmDataset.imageStore.push(payload);
    },
    
    /* 
    * SUBSETS
    */
    reverse: (state) => state.dsmSubsets.reverse(),
     
    addSubset: (state, payload: T.ImageList) => {
        state.dsmSubsets.push(payload);
    },

    removeSubset:  (State, guid) => {
        state.dsmSubsets.forEach( (subset, index) => {
            if(subset.guid == guid){
                state.dsmSubsets.splice(index, 1);
            }
        });
    },

    /* 
    * TESTS
    */
    addTest: (state, payload: T.Test) => {
    state.tmTests.push(payload);
    },

    removeTest:  (State, guid) => {
        state.tmTests.forEach( (test, index) => {
            if(test.guid == guid){
                state.tmTests.splice(index, 1);
            }
        });
    },

    updateView: (state, payload: T.DsmView) => {
        state.dsmView = payload;
    }

}

declare var CefSharp: BindingObject.CefSharp;
declare var boundDataSet: BindingObject.DataSet;

const actions: ActionTree<State,any> = {

    fetchS3Images: async (state) => {

        await CefSharp.BindObjectAsync("boundDataSet", "boundDataSet");

        console.log("Sending");
        await boundDataSet.getImageArray().then(function (res: T.ImageElement[])
        {

            console.log("CEF Response: ");
            
            res.forEach( (index) => {
                console.log("Adding: " + index.name);  
                state.getters.getDataSet.imageStore.push(index);
            });
        });

    }
}

const state: State = {

    dsmDataset: {
        guid: "123456789",
        name: "Dataset",
        imageStore: [
            {
                guid: Global.newGuid(),
                name: "james.jpg",
                url: "dataset/james.jpg"
            },
            {
                guid: Global.newGuid(),
                name: "ashton.jpg",
                url: "dataset/ashton.jpg"
            },
            {
                guid: Global.newGuid(),
                name: "jason.jpg",
                url: "dataset/jason.jpg"
            },
            {
                guid: Global.newGuid(),
                name: "vinura.jpg",
                url: "dataset/vinura.jpg"
            }
        ],
    },
    dsmSubsets: [],

    tmTests: [],

    dsmView: {
        edit: false,
        guid: undefined
    }
}

export default new Vuex.Store<State>({
    state,
    mutations,
    getters,
    actions
});
