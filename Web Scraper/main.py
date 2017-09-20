import Laptop
import Desktop

import time
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup


def main():
    input("Press ENTER to start...")

    start_time = time.clock()

    products = [Laptop.Laptop(), Desktop.Desktop()]

    for product in products:
        product.run()

    print("\n{0}s".format(time.clock() - start_time))


if __name__ == "__main__":
    main()
