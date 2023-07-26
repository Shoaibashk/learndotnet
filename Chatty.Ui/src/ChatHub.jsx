import signalR from '@microsoft/signalr'

const ChatHub = () => {

    let connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

    connection.on("send", data => {
        console.log(data);
    });

    connection.start()
    .then(() => connection.invoke("send", "Hello"));
  return (
    <>
    <div>ChatHub</div>
    </>
  )
}

export default ChatHub