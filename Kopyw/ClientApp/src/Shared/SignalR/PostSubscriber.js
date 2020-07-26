import { HubConnectionBuilder } from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
    .withUrl("/posthub")
    .withAutomaticReconnect()
    .build();

const PostSubscriber = {
    connection: connection,
    isConnected: false
}

const onStart = () => {
    PostSubscriber.isConnected = true;
}

connection.start()
    .then(() => {
        onStart();
    })
    .catch(err => console.error(err));

export default PostSubscriber;