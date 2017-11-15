import re
from urllib.request import urlopen
from bs4 import BeautifulSoup as soup
from multiprocessing import Pool


class Product(object):
    def __init__(self):
        self.page = "https://www.coolblue.nl"

    def start(self, filename, headers):
        page_soup_first = self.bsPage(self.page)

        page_count_buffer = page_soup_first.find(
            "ul", {"class": "pagination js-pagination"}).findAll("li")
        page_count = int(page_count_buffer[len(
            page_count_buffer) - 2].text.strip())

        file = open(filename, "w")
        file.write(headers + "\n")
        file.close()

        return page_count

    def bsPage(self, page):
        Client = urlopen(page)
        page_html = Client.read()
        Client.close()

        return soup(page_html, "html5lib")

    def getPageData(self, pageNr):
        page_soup = self.bsPage(self.page + "?pagina={0}".format(pageNr + 1))
        cards = page_soup.findAll(
            "a", {"class": "product__title js-product-title"})
        card_links = [[i, cards[i].parent.parent.parent.find(
            "div", {"class": "product__display"}).a["data-producttypename"].replace("\"", "''"), cards[i]["href"]] for i in range(len(cards))]

        print("\t{0}?pagina={1}".format(self.page, pageNr + 1))

        p = Pool(10)
        p.map(self.getCardData, [card_link for card_link in card_links])
        p.terminate()
        p.join()

    def getSpec(self, card_detail, spec, desc=False):
        if (desc):
            try:
                return card_detail.find("div", {"class", "product-description--content cms-content js-product-description-content"}).p.text.strip()
            except:
                return "None"
        else:
            try:
                return card_detail.find(text=re.compile(spec)).parent.parent.dd.text.strip().replace("\"", "''")
            except:
                try:
                    return card_detail.find(text=re.compile(spec)).parent.parent.parent.parent.dd.text.strip().replace("\"", "''")
                except:
                    try:
                        return card_detail.find(text=re.compile(spec)).parent.dd.text.strip().replace("\"", "''")
                    except:
                        return "None"

    def addSpec(self, specs, spec):
        specs.append("\"{0}\"".format(spec.replace("\"", "''")))

    def removeSymbols(self, name):
        symbols = "!@#$%^&*+=:;\"\'<,>.?/|\\`~"

        for symbol in symbols:
            name = name.replace(symbol, "")

        return name

    def getImages(self, card_detail, product_specs):
        images = card_detail.findAll(
            "img", {"class": "media-gallery--thumbnail-image js-media-gallery--thumbnail"})
        res = ""

        for i in range(len(images)):
            images[i] = images[i]["src"][:41]
            res = res + images[i] + ">>"

            # imagename = "Data/Images/{0} - ".format(self.removeSymbols(
            #     product_specs[1])) + "0000{0}.jpg"[len(str(i)):].format(i)
            # image = open(imagename, "wb")
            # image.write(urlopen(images[i]).read())
            # image.close()

        return res

    def getCardData(self, card_link):
        pass
