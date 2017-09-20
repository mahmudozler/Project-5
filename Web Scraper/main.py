import Laptop

import time
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup


def main():
    input("Press ENTER to start...")

    start_time = time.clock()
    count = 0

    products = [Laptop.Laptop()]

    for product in products:
        product.run()
        count = count + product.count

    print("found {0} products".format(count))
    print("{0}s".format(time.clock() - start_time))

if __name__ == "__main__":
    main()
