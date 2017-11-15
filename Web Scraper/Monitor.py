import re
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup
from multiprocessing import Pool

import Product


class Monitor(Product.Product):
    def __init__(self):
        self.page = "https://www.coolblue.nl/producttype:monitoren"

    def run(self):
        headers = "product_type,product_name,product_desc,product_price,product_brand,product_model,product_screen,product_screenres,product_response,product_brightness,product_refresh,product_screentype,product_connection,product_dimensions,product_weight,product_images"
        page_count = self.start("Data/product_monitors.csv", headers)

        for i in range(page_count):
            self.getPageData(i)

    def getCardData(self, card_link):
        if len(card_link[2]) < 16:
            card_detail = self.bsPage(self.page[:23] + card_link[2])
            product_specs = [card_link[1]]

            self.addSpec(product_specs,
                         card_detail.h1.text.strip())
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "", True))
            self.addSpec(product_specs,
                         card_detail.find("strong", {"class": "sales-price--current"}).text.strip())
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Merk"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Fabrikantcode"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Schermdiagonaal"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Resolutie"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Responstijd"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Helderheid (nit)"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Beeldverversing"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Schermtype"))
            self.addSpec(product_specs,
                         self.getSpec(card_detail, "Aansluitingen"))
            self.addSpec(product_specs, "{0} x {1} x {2}".format(
                self.getSpec(card_detail, "Breedte"), self.getSpec(card_detail, "Diepte"), self.getSpec(card_detail, "Hoogte")))
            self.addSpec(product_specs, self.getSpec(
                card_detail, "Gewicht"))
            self.addSpec(product_specs, self.getImages)

            filename = "Data/product_monitors.csv"
            file = open(filename, "a")
            file.write(",".join(product_specs) + "\n")
            file.close()

            self.getImages(card_detail, product_specs)

            print(product_specs[0] + "\tproduct_name: " + product_specs[1])
