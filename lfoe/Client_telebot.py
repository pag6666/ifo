import os
import socket
import telebot
import time
from telebot import types
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
token = "6854657741:AAHOCB482AGmSUn5LNDmuN6bVfy37gFV_MQ"
bot = telebot.TeleBot(token)
def read(byte_arr):
     return str(byte_arr, encoding = 'utf-8')
     
users = []
@bot.message_handler(commands=["start"],content_types=["text"])
def Start(message):
    markup = types.ReplyKeyboardMarkup(True)
    
    client.connect(("127.0.0.1" , 25565))
    client.send(b"Telebot")
    data = client.recv(1000)
    print(read(data))
    button1 = types.KeyboardButton("telebot_conected")
    markup.add(button1)
    bot.send_message(message.chat.id,"hello user kmept",reply_markup = markup)
    while(True):
        command = read(client.recv(1000))
        if(command == "users"):
            client.send(b"users")
        elif(command == "telebot_connect"):
            print("server see me")
            client.send(b"telebot_connected")
            while(True):
                
                text:str = message.text
                if(text == "telebot_conected"):
                    client.send(text.encode('utf-8'))
                elif(text == "users"):
                    client.send(b"users")
                else:
                    client.send(b"null")
                    time.sleep(1)
        else:
            client.send(b"ERROR ANSWER")

        

        

bot.infinity_polling()
