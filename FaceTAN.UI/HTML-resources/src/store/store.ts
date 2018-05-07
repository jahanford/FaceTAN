import Vue from 'vue';
import Vuex from 'vuex';
import { GetterTree, MutationTree, ActionTree,  } from 'vuex';
import * as T from '../models/models';
import * as BindingObject from '../models/BindingObject';

Vue.use(Vuex)

interface State {  
    
    dsmDataset: T.ImageList;
    dsmSubsets: T.ImageList[];

    dsmView: T.DsmView;
}

const getters: GetterTree<State, any> = {
    
    getDataSet: (state, getters) => {
        return state.dsmDataset;
    },
    getSubsets: (state, getters) => {
        return state.dsmSubsets;
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

function newGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0, v = c === 'x' ? r : ( r & 0x3 | 0x8 );
        return v.toString(16);
    });
}

const state: State = {

    dsmDataset: {
        guid: "123456789",
        name: "Dataset",
        imageStore: [
            {
                guid: newGuid(),
                name: "james.jpg",
                url: "dataset/james.jpg"
            },
            {
                guid: newGuid(),
                name: "ashton.jpg",
                url: "dataset/ashton.jpg"
            },
            {
                guid: newGuid(),
                name: "jason.jpg",
                url: "dataset/jason.jpg"
            },
            {
                guid: newGuid(),
                name: "vinura.jpg",
                url: "dataset/vinura.jpg"
            }
        ],
    },
    dsmSubsets: [],
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
