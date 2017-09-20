import re
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup
from multiprocessing import Pool

import Product


class Laptop(Product.Product):
    def __init__(self):
        self.page = "https://www.coolblue.nl/producttype:laptops"
        self.count = 0

        self.product_specs = ["Laptop"]

    def run(self):
        filename = "Data/product_laptops.csv"
        file = open(filename, "w")
        file.write("product_type,product_name,product_price,product_brand,product_model,product_screen,product_screenres,product_cpu,product_graphicscard,product_graphicsmemory,product_memory,product_storage,product_dimensions,product_weight\n")

        page_soup_first = self.bsPage(self.page)

        cards = page_soup_first.findAll(
            "div", {"class": "product-grid__item card"})
        page_count_buffer = page_soup_first.find(
            "ul", {"class": "pagination js-pagination"}).findAll("li")
        page_count = int(page_count_buffer[len(
            page_count_buffer) - 2].text.strip())

        for i in range(page_count):
            self.getPageData(i)

        file.write(",".join(self.product_specs) + "\n")
        file.close()

    def getCardData(self, card_link):
        card_detail = self.bsPage(self.page[:23] + card_link)
        self.count = self.count + 1

        self.addSpec(
            card_detail.h1.text.strip())
        self.addSpec(
            card_detail.find("strong", {"class": "sales-price--current"}).text.strip())
        self.addSpec(
            self.getSpec(card_detail, "Merk"))
        self.addSpec(
            self.getSpec(card_detail, "Fabrikantcode"))
        self.addSpec(
            self.getSpec(card_detail, "Schermdiagonaal"))
        self.addSpec(
            self.getSpec(card_detail, "Resolutie"))
        self.addSpec(
            self.getSpec(card_detail, "Processor") + " " + self.getSpec(card_detail, "Processornummer"))
        self.addSpec(
            self.getSpec(card_detail, "Videokaart"))
        self.addSpec(
            self.getSpec(card_detail, "Videogeheugen"))
        self.addSpec(
            self.getSpec(card_detail, "RAM-geheugen"))
        self.addSpec(
            self.getSpec(card_detail, "Opslagcapaciteit"))
        self.addSpec("{0} x {1} x {2}".format(self.getSpec(card_detail, "Breedte"),
                                              self.getSpec(
                                                  card_detail, "Diepte"),
                                              self.getSpec(
                                                  card_detail, "Hoogte")))
        self.addSpec(self.getSpec(
            card_detail, "Gewicht"))

        print("product_name: " + self.product_specs[1])
