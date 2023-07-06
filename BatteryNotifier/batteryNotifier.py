import psutil
import time
import tkinter as tk

def show_popup(window_title, window_message):
        window_size = "600x200+0+0"
        
        # Create a pop-up window
        window = tk.Tk()
        window.title(window_title)
        window.geometry(window_size)

        # Add a message to the pop-up window
        message = tk.Label(window, text=window_message, font="TkDefaultFont 10 bold")
        message.pack()

        # Add an icon to the pop-up window
        # window.iconbitmap("./batteryNotifier.ico")

        # Make the pop-up window stay on top of everything
        window.wm_attributes("-topmost", True)

        # Display the pop-up window
        window.mainloop()

# window title and message
charged_window_title = "Battery Charged!"
charged_window_message = """Your battery is now fully charged. You can now unplug your device.\n\n
Tips to prolong battery life:\n* Do not charge your battery to 100%.\n* Don't leave your laptop plugged in all the time."""

chargeNeed_window_title = "Battery need to be charged. HURRY!!!"
chargeNeed_window_message = """Your battery needs to be charged. You can now plug in your device.\n\n
Tips to prolong battery life:\n* Do not charge your battery to 100%.\n* Don't leave your laptop plugged in all the time."""

# Check battery percentage continuously
while True:
    battery = psutil.sensors_battery()
    plugged = battery.power_plugged
    percent = battery.percent

    # Display notification when battery reaches specified %
    if plugged and percent >= 90:
        show_popup(charged_window_title, charged_window_message)

    if not plugged and percent <= 20:
        show_popup(chargeNeed_window_title, chargeNeed_window_message) 

    time.sleep(60)  # Sleep for 60 seconds