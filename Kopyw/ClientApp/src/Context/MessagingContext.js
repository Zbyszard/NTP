import React from 'react';

const MessagingContext = React.createContext({
    conversations: [],
    loadingConversationIds: [],
    activeConversationId: 0,
    areConversationsLoading: false,
    enterConversation: convId => { },
    exitConversation: () => { },
    requestMessages: (convId, olderThan) => { },
    setConversationInput: (convId, value) => { },
    sendMessage: messageObj => { }
});

export default MessagingContext;