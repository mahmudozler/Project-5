import Laptop
import Desktop
import Monitor
import Printer
import Mouse
import Keyboard

import time
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup


def main():
    input("Press ENTER to start...")

    start_time = time.clock()

    products = [Laptop.Laptop(), Desktop.Desktop(), Monitor.Monitor(), Printer.Printer(), Mouse.Mouse(), Keyboard.Keyboard()]

    for product in products:
        product.run()

    print("\n{0}s".format(time.clock() - start_time))


if __name__ == "__main__":
    main()
