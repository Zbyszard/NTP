import { HubConnectionBuilder } from '@microsoft/signalr';
import authService from '../../components/api-authorization/AuthorizeService';

const instances = {};

class Subscriber {
    isConnected = false;
    connection = undefined;

    static getSubscriber = hubUrl => {
        if (instances[hubUrl])
            return instances[hubUrl];
        let instance = new Subscriber();
        instance.connection = new HubConnectionBuilder()
            .withUrl(hubUrl, { accessTokenFactory: async () => await authService.getAccessToken() })
            .withAutomaticReconnect()
            .build();
        instance.connection.start()
            .then(() => instance.onConnected())
            .catch(err => {
                //TODO: refresh token
                console.error(err);
                throw err;
            });
        instance.connection.onreconnecting(() => instance.isConnected = false);
        instance.connection.onreconnected(() => instance.onConnected());
        instance.connection.onclose(() => instance.isConnected = false);
        instances[hubUrl] = instance;
        return instance;
    }

    onConnected = () => {
        this.isConnected = true;
        this.onReady();
        this.onReadyCallbacks = [];
    }

    onReady = () => {
        if (this.onReadyCallbacks)
            this.onReadyCallbacks.forEach(cb => cb());
    }

    onReadyCallbacks = []
    addOnReadyCallback = callback => {
        if (typeof callback !== "function")
            throw new Error(`Type error: expected function, got ${typeof callback}`);
        this.onReadyCallbacks.push(callback);
    }
}

export default Subscriber;